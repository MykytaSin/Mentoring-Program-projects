using EventApi.DTO;

namespace EventApi.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderInfo> GetOrderAsync(Guid cartIdentifier);
        public Task<OrderInfo> AddNewTicketToOrderAsync(Guid cartIdentifier, CartTicketData ticketData);
        public Task<OrderInfo> DeletTicketAsync(Guid cart_id, int event_id, int seat_id);
        public Task<Guid> MooveTicketsInCartToBookedStatusAsync(Guid cartIdentifier);
    }
}
