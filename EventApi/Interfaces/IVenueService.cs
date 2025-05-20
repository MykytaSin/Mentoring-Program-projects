using DAL.Models;
using EventApi.DTO;

namespace EventApi.Interfaces
{
    public interface IVenueService
    {
        public Task<List<Section>> GetAllVenuesSection(int venueId);
        public Task<List<VenueInfo>> GetAllVenues();
    }
}
