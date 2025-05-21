using DAL.Models;
using EventApi.DTO;
using EventApi.Helpers;
using EventApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EventApi.Services
{
    public class EventService : IEventService
    {
        private readonly IMapHelper _mapHelper;
        private readonly MyAppContext _context;
        private readonly IMemoryCache _cache;
        private readonly ICacheHelper _cacheHelper;

        public EventService(IMapHelper mapHelper, MyAppContext context, IMemoryCache memoryCache, ICacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
            _cache = memoryCache;
            _mapHelper = mapHelper;
            _context = context;
        }
        public async Task<List<Event>> GetAllEvents()
        {
            if (!_cache.TryGetValue(Constants.AllEventsCacheKey, out List<Event> events))
            {
                events = await _context.Events.ToListAsync();

                var cacheOptions = _cacheHelper.GetDefaultCacheOptions();

                _cache.Set(Constants.AllEventsCacheKey, events, cacheOptions);
            }

            return events;
        }
        public async Task<List<EventInfo>> GetAllMinimizedEventInfo()
        {

            if (!_cache.TryGetValue(Constants.MinimizedEventsCacheKey, out List<EventInfo> eventsInfo))
            {
                var events = await _context.Events.Include(e => e.Eventstatus)
                .ToListAsync();
                eventsInfo = await _mapHelper.MapEventsToEventsInfoAsync(events);

                var cacheOptions = _cacheHelper.GetDefaultCacheOptions();
                _cache.Set(Constants.MinimizedEventsCacheKey, eventsInfo, cacheOptions);
            }

            return eventsInfo;
        }
        public async Task<List<SeatsWithStatusAndPrice>> GetSeatsWithStatusAndPriceOptions(int eventId, int sectionId)
        {

            var key = _cacheHelper.GetDynamicKey(Constants.SeatsWithStatusAndPriceCacheKey, eventId.ToString(), sectionId.ToString());


            if (!_cache.TryGetValue(key, out List<SeatsWithStatusAndPrice> seatsWithDetails))
            {
                seatsWithDetails = await _context.Seats
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

                var cacheOptions = _cacheHelper.GetDefaultCacheOptions();

                _cache.Set(key, seatsWithDetails, cacheOptions);
            }

            return seatsWithDetails;
        }
    }
}
