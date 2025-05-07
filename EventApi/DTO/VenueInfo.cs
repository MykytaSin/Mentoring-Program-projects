namespace EventApi.DTO
{
    public class VenueInfo
    {
        public int VenueId { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public int? Capacity { get; set; }

    }
}
