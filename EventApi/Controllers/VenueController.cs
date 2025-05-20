using EventApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenueController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        // GET /venues
        [HttpGet]
        public async Task<IActionResult> GetAllVenues()
        {
            var venues = await _venueService.GetAllVenues();
            return Ok(venues);
        }

        // GET /venues/{venue_id}/sections
        [HttpGet("{venueId}/sections")]
        public async Task<IActionResult> GetAllVenueSections(int venueId)
        {
            var sections = await _venueService.GetAllVenuesSection(venueId);
            return Ok(sections);
        }
    }
}
