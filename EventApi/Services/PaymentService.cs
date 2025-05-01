using EventApi.DTO;
using EventApi.Interfaces;

namespace EventApi.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<Guid> GetPaymentId()
        {
            return await Task.Run(() => Guid.NewGuid());
        }
    }
}
