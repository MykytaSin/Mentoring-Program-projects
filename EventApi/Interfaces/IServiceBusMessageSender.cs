namespace EventApi.Interfaces
{
    public interface IServiceBusMessageSender : IAsyncDisposable
    {
        Task SendMessageAsync<T>(T messageBody, CancellationToken cancellationToken = default);
        Task SendMessagesBatchAsync<T>(IEnumerable<T> messagesBody, CancellationToken cancellationToken = default);
    }
}
