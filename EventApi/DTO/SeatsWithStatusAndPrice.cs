using DAL.Models;

namespace EventApi.DTO
{
    public class SeatsWithStatusAndPrice
    {
        public int SectionId { get; set; }
        public int RowNumber { get; set; }
        public int SeatId { get; set; }
        public Ticketstatus? Status { get; set; }
        public List<MinimizedPriceOprions> PriceOptions { get; set; } = new List<MinimizedPriceOprions>();
    }
}
