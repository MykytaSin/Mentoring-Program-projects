using DAL.Models;

namespace EventApi.Interfaces
{
    public interface IPaymentService
    {
        public Guid GetPaymentId();
        public Task<string> GetPaymentStatus(Guid payment_id);
        public Task<bool> CompletePayment(Guid payment_id);
        public Task<bool> RollBackPayment(Guid payment_id);
        public Task<bool> SetPaymentStatus(Payment payment, string status);
        public Task<bool> UpdateSeatStatus(Payment payment, string status);
    }
}
