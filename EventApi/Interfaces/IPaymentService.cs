namespace EventApi.Interfaces
{
    public interface IPaymentService
    {
        public Task<Guid> GetPaymentId();
    }
}
