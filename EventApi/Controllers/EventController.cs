using EventApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // a. GET /events
        [HttpGet]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllMinimizedEventInfo();
            return Ok(events);
        }

        // b. GET /events/{event_id}/sections/{section_id}/seats
        [HttpGet("{event_id}/sections/{section_id}/seats")]
        [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> GetSeatsWithStatusAndPriceOptions(int event_id, int section_id)
        {
            var seats = await _eventService.GetSeatsWithStatusAndPriceOptions(event_id, section_id);
            return Ok(seats);
        }
    }
}