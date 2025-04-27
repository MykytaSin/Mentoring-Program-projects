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
        private readonly IAsyncRepository<Seat> _seatsRepository;
        private readonly IMapHelper _mapHelper;
        private readonly MyAppContext _context;
        public EventService(IUnitOfWork unitOfWork, IMapHelper mapHelper, MyAppContext context)
        {
            _unitOfWork = unitOfWork;
            _eventRepository = _unitOfWork.Repository<Event>();
            _seatsRepository = _unitOfWork.Repository<Seat>();
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

        //public async Task<List<object>> GetSeatsWithStatus(int eventId, int sectionId)
        //{
        //    var seatsWithStatus = await _seatsRepository.GetAllAsync(
        //        s => s.SectionId == sectionId && s.Tickets.Any(t => t.Eventid == eventId),
        //        null,
        //        s => s.Tickets,
        //        s => s.Tickets.Select(t => t.Ticketstatus)
        //    ).Select(s => new
        //    {
        //        SectionId = s.SectionId,
        //        RowId = s.Rownumber,
        //        SeatId = s.Seatid,
        //        Status = new
        //        {
        //            Id = s.Tickets.FirstOrDefault() != null ? s.Tickets.FirstOrDefault().Ticketstatus.Ticketstatusid : (int?)null,
        //            Name = s.Tickets.FirstOrDefault() != null ? s.Tickets.FirstOrDefault().Ticketstatus.Ticketstatusname : null
        //        }
        //    }).ToListAsync();

        //    return seatsWithStatus;
        //}

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
