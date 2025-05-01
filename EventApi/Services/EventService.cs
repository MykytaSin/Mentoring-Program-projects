using DAL.Interfaces;
using DAL.Models;
using EventApi.DTO;
using EventApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAsyncRepository<Event> _eventRepository;
        private readonly IMapHelper _mapHelper;
        private readonly MyAppContext _context;
        public EventService(IUnitOfWork unitOfWork, IMapHelper mapHelper, MyAppContext context)
        {
            _unitOfWork = unitOfWork;
            _eventRepository = _unitOfWork.Repository<Event>();
            _mapHelper = mapHelper;

            _context = context;
        }


        public async Task<List<Event>> GetAllEvents()
        {
            var events = await _eventRepository.GetAllAsync().ToListAsync();
            return events;
        }

        public async Task<List<EventInfo>> GetAllMinimizedEventInfo()
        {
            var events = await _eventRepository.GetAllAsync(
                null,
                null,
                e => e.Eventstatus
                ).ToListAsync();

            return await _mapHelper.MapEventsToEventsInfoAsync(events);
        }

        public async Task<List<object>> GetSeatsWithStatusAndPriceOptions(int eventId, int sectionId)
        {
            var seatsWithDetails = await _context.Seats
                .Where(s => s.Sectionid == sectionId)
                .Select(s => new
                {
                    SectionId = s.Sectionid,
                    RowId = s.Rownumber,
                    SeatId = s.Seatid,
                    Status = s.Tickets
                        .Where(t => t.Eventid == eventId)
                        .Select(t => new
                        {
                            Id = t.Ticketstatus.Ticketstatusid,
                            Name = t.Ticketstatus.Ticketstatusname
                        })
                        .FirstOrDefault(),
                    PriceOptions = s.Tickets
                        .Where(t => t.Eventid == eventId)
                        .SelectMany(t => t.Offerprice.Offer.Offerprices)
                        .Select(op => new
                        {
                            Id = op.Offerpriceid,
                            Name = $"Price Level {op.Pricelevelid}",
                            Price = op.Price
                        })
                        .ToList()
                })
                .ToListAsync();



            return seatsWithDetails.Cast<object>().ToList();
        }


    }
}
