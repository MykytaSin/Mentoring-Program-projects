namespace EventApi.DTO
{
    public class TicketMinimizedInfo
    {
        public int TicketId { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate{ get; set; }
        public string SectionName { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        
        public decimal Price { get; set; }
    }
}
