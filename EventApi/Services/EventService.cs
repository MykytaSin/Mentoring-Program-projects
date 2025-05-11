using DAL.Models;
using EventApi.DTO;
using EventApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Services
{
    public class EventService : IEventService
    {
        private readonly IMapHelper _mapHelper;
        private readonly MyAppContext _context;
        public EventService(IMapHelper mapHelper, MyAppContext context)
        {
            _mapHelper = mapHelper;
            _context = context;
        }
        public async Task<List<Event>> GetAllEvents()
        {
            var events = await _context.Events.ToListAsync();
            return events;
        }
        public async Task<List<EventInfo>> GetAllMinimizedEventInfo()
        {
            var events = await _context.Events.Include(e => e.Eventstatus)
                .ToListAsync();

            return await _mapHelper.MapEventsToEventsInfoAsync(events);
        }
        public async Task<List<SeatsWithStatusAndPrice>> GetSeatsWithStatusAndPriceOptions(int eventId, int sectionId)
        {
            var seatsWithDetails = await _context.Seats
                .Where(s => s.Sectionid == sectionId)
                .Select(s => new SeatsWithStatusAndPrice
                {
                    SectionId = s.Sectionid,
                    RowNumber = s.Rownumber,
                    SeatId = s.Seatid,
                    Status = s.Tickets
                        .Where(t => t.Eventid == eventId)
                        .Select(t => t.Ticketstatus)
                        .FirstOrDefault(),
                    PriceOptions = s.Tickets
                        .Where(t => t.Eventid == eventId)
                        .SelectMany(t => t.Offerprice.Offer.Offerprices)
                        .Select(op => new MinimizedPriceOprions
                        {
                            Id = op.Offerpriceid,
                            Name = $"Price Level {op.Pricelevelid}",
                            Price = op.Price
                        })
                        .ToList()
                })
                .ToListAsync();

            return seatsWithDetails;
        }
    }
}
