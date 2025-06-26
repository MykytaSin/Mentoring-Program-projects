using System.Collections.Concurrent;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using EventApi.Helpers;
using EventApi.Interfaces;
using Microsoft.Extensions.Options;

namespace EventApi.Services
{
    public class ServiceBusMessageReceiver : IServiceBusMessageReceiver
    {
        private readonly ServiceBusClient _client;
        private readonly ILogger<ServiceBusMessageReceiver> _logger;
        private readonly ServiceBusSettings _settings;

        private readonly ConcurrentDictionary<string, ServiceBusProcessor> _processors = new ConcurrentDictionary<string, ServiceBusProcessor>();

        public ServiceBusMessageReceiver(
            IOptions<ServiceBusSettings> settings,
            ILogger<ServiceBusMessageReceiver> logger)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = new ServiceBusClient(_settings.ConnectionString ?? throw new ArgumentNullException(nameof(settings.Value.ConnectionString)));
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception,
                             "Error occurred while processing messages from queue {QueueName}. Source: {ErrorSource}, Namespace: {Namespace}, Entity Path: {EntityPath}",
                             args.EntityPath, args.ErrorSource, args.FullyQualifiedNamespace, args.EntityPath);
            return Task.CompletedTask;
        }

        public async Task StartReceivingAsync<T>(string queueName, Func<T, CancellationToken, Task> messageProcessor, CancellationToken cancellationToken = default) where T : class
        {
            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentNullException(nameof(queueName));
            }
            if (messageProcessor == null)
            {
                throw new ArgumentNullException(nameof(messageProcessor));
            }

            if (_processors.TryGetValue(queueName, out var existingProcessor) && existingProcessor.IsProcessing)
            {
                _logger.LogWarning("Receiver for queue {QueueName} is already running.", queueName);
                return;
            }

            var processor = _client.CreateProcessor(queueName, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = _settings.MaxConcurrentCalls,
                AutoCompleteMessages = false
            });

            processor.ProcessErrorAsync += ErrorHandler;
            processor.ProcessMessageAsync += async args => await HandleReceivedMessage(args, messageProcessor, cancellationToken, queueName);

            if (!_processors.TryAdd(queueName, processor))
            {
                _logger.LogError("Failed to add processor for queue {QueueName} to dictionary. It might already exist or a race condition occurred.", queueName);
                await processor.DisposeAsync();
                throw new InvalidOperationException($"Processor for queue {queueName} could not be added.");
            }

            try
            {
                await processor.StartProcessingAsync(cancellationToken);
                _logger.LogInformation("Started receiving messages from queue {QueueName}.", queueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting message receiver for queue {QueueName}.", queueName);
                _processors.TryRemove(queueName, out _);
                await processor.DisposeAsync();
                throw;
            }
        }

        private async Task HandleReceivedMessage<T>(ProcessMessageEventArgs args, Func<T, CancellationToken, Task> messageProcessor, CancellationToken cancellationToken, string queueName) where T : class
        {
            string body = args.Message.Body.ToString();
            _logger.LogDebug("Received message from {QueueName}: Id = {MessageId}, Body = {MessageBody}", queueName, args.Message.MessageId, body);

            try
            {
                T? messageBody = JsonSerializer.Deserialize<T>(body);
                if (messageBody != null)
                {
                    await messageProcessor(messageBody, cancellationToken);
                    await args.CompleteMessageAsync(args.Message, cancellationToken);
                    _logger.LogInformation("Successfully processed and completed message Id: {MessageId} from {QueueName}", args.Message.MessageId, queueName);
                }
                else
                {
                    _logger.LogError("Failed to deserialize message Id: {MessageId} from {QueueName}. Body: {MessageBody}. Dead-lettering message.", args.Message.MessageId, queueName, body);
                    await args.DeadLetterMessageAsync(args.Message, "Deserialization Error", "Message body could not be deserialized.");
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON deserialization error for message Id {MessageId} from {QueueName}. Dead-lettering message.", args.Message.MessageId, queueName);
                await args.DeadLetterMessageAsync(args.Message, "JSON Deserialization Error", jsonEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message Id {MessageId} from {QueueName}. Abandoning message.", args.Message.MessageId, queueName);
                await args.AbandonMessageAsync(args.Message, null, cancellationToken);
            }
        }

        public async Task StopReceivingAsync(string queueName)
        {
            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentNullException(nameof(queueName));
            }

            if (_processors.TryRemove(queueName, out var processor))
            {
                if (processor.IsProcessing)
                {
                    _logger.LogInformation("Stopping message receiver for queue {QueueName}...", queueName);
                    await processor.StopProcessingAsync();
                    _logger.LogInformation("Message receiver for queue {QueueName} stopped.", queueName);
                }
                await processor.DisposeAsync();
            }
            else
            {
                _logger.LogWarning("No active receiver found for queue {QueueName} to stop.", queueName);
            }
        }

        public async Task StopAllReceiversAsync()
        {
            _logger.LogInformation("Stopping all active message receivers...");
            var tasks = new List<Task>();
            foreach (var kvp in _processors)
            {
                tasks.Add(StopReceivingAsync(kvp.Key));
            }
            await Task.WhenAll(tasks);
            _logger.LogInformation("All message receivers stopped.");
        }

        public async ValueTask DisposeAsync()
        {
            _logger.LogInformation("Disposing ServiceBusMessageReceiver (main client) for all queues.");
            await StopAllReceiversAsync();

            if (_client != null)
            {
                await _client.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }

    }
}

