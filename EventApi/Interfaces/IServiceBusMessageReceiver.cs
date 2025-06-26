namespace EventApi.Interfaces
{
    public interface IServiceBusMessageReceiver : IAsyncDisposable
    {
        Task StartReceivingAsync<T>(string queueName, Func<T, CancellationToken, Task> messageProcessor, CancellationToken cancellationToken = default) where T : class;
        Task StopReceivingAsync(string queueName);
        Task StopAllReceiversAsync();
    }   
}