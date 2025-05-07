using DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventApi.DTO
{
    public class EventInfo
    {
        public int EventId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime EventDateTime { get; set; }
        public int Eventstatusid { get; set; }
        public string EventStatusName { get; set; }
        public string? Description { get; set; }
    }
}
