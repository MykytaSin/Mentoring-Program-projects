using DAL.Interfaces;
using DAL.Models;
using EventApi.DTO;
using EventApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Services
{
    public class VenueService: IVenueService
    {
        IUnitOfWork _unitOfWork;
        IAsyncRepository<Venue> _venueRepo;
        IMapHelper _mapHelper;

        public VenueService(IUnitOfWork unitOfWork, IMapHelper mapHelper)
        {
            _unitOfWork = unitOfWork;

            _venueRepo = _unitOfWork.Repository<Venue>();
            _mapHelper = mapHelper;
        }
        public async Task<List<VenueInfo>> GetAllVenues()
        {
            var venues = await _venueRepo.GetAllAsync().ToListAsync();
            return await _mapHelper.MapVenuesToVenueInfoAsync(venues);
        }

        public async Task GetAllVenuesSection(int venueId)
        {
            var venueWithManifest = _venueRepo.GetAllAsync(
                predicate: venue => venue.Venueid == venueId,
                null,
                venue => venue.Manifests.Select(manifest => manifest.Manifesttype),
                venue => venue.Manifests.Select(manifest => manifest.Seats)
            ).FirstOrDefaultAsync();
        }



    }
}
