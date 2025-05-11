using DAL.Interfaces;
using DAL.Models;
using EventApi.DTO;
using EventApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Services
{
    public class VenueService: IVenueService
    {
        IMapHelper _mapHelper;
        MyAppContext _context;

        public VenueService( IMapHelper mapHelper, MyAppContext context)
        {
            _context = context;
            _mapHelper = mapHelper;
        }
        public async Task<List<VenueInfo>> GetAllVenues()
        {
            var venues = await _context.Venues.ToListAsync();
            return await _mapHelper.MapVenuesToVenueInfoAsync(venues);
        }

        public async Task<List<Section>> GetAllVenuesSection(int venueId)
        {
            var sections = await _context.Venues
                .Include(v=>v.Manifests)
                .ThenInclude(m => m.Sections)
                .Where(v=>v.Venueid==venueId)
                .SelectMany(m=>m.Manifests)
                .SelectMany(s=>s.Sections).ToListAsync();
            return sections;
        }
    }
}
