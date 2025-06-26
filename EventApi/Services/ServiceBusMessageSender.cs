using System.Text.Json;
using Azure.Messaging.ServiceBus;
using EventApi.Interfaces;

namespace EventApi.Services
{
    public class ServiceBusMessageSender : IServiceBusMessageSender
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private readonly string _queueName;

        public ServiceBusMessageSender(string connectionString, string queueName)
        {
            _queueName = queueName?? throw new ArgumentNullException(nameof(queueName));
            _client = new ServiceBusClient(connectionString ?? throw new ArgumentNullException(nameof(connectionString)));
            _sender = _client.CreateSender(_queueName);
        }

        public async Task SendMessageAsync<T>(T messageBody, CancellationToken cancellationToken = default)
        {
            if (messageBody == null)
            {
                throw new ArgumentNullException(nameof(messageBody));
            }

            string jsonMessage = JsonSerializer.Serialize(messageBody);
            ServiceBusMessage message = new ServiceBusMessage(jsonMessage);

            try
            {
                await _sender.SendMessageAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error sending message to queue {_queueName}: {ex.Message}");
                throw;
            }
        }

        public async Task SendMessagesBatchAsync<T>(IEnumerable<T> messagesBody, CancellationToken cancellationToken = default)
        {
            if (messagesBody == null)
            {
                throw new ArgumentNullException(nameof(messagesBody));
            }

            ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync(cancellationToken);

            foreach (var item in messagesBody)
            {
                string jsonMessage = JsonSerializer.Serialize(item);
                ServiceBusMessage message = new ServiceBusMessage(jsonMessage);

                if (!messageBatch.TryAddMessage(message))
                {

                    try
                    {
                        await _sender.SendMessagesAsync(messageBatch, cancellationToken);
                        Console.WriteLine($"Sent a batch of {messageBatch.Count} messages to queue {_queueName}.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending message batch to queue {_queueName}: {ex.Message}");
                        throw;
                    }

                    messageBatch.Dispose();
                    messageBatch = await _sender.CreateMessageBatchAsync(cancellationToken);
                    if (!messageBatch.TryAddMessage(message))
                    {
                        Console.Error.WriteLine($"Message too large to fit in an empty batch. Skipping message: {jsonMessage}");
                        continue;
                    }
                }
            }

            if (messageBatch.Count > 0)
            {
                try
                {
                    await _sender.SendMessagesAsync(messageBatch, cancellationToken);
                    Console.WriteLine($"Sent a final batch of {messageBatch.Count} messages to queue {_queueName}.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending final message batch to queue {_queueName}: {ex.Message}");
                    throw;
                }
            }

            messageBatch.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_sender != null)
            {
                await _sender.DisposeAsync();
            }
            if (_client != null)
            {
                await _client.DisposeAsync();
            }
            GC.SuppressFinalize(this);
        }
    }
}
