namespace EventApi.DTO
{
    public class OrderInfo
    {
        public Guid CartIdentifier { get; set; }
        public List<TicketMinimizedInfo> MinimizedTickets { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
