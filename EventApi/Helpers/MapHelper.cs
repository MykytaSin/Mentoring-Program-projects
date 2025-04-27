using DAL.Models;
using EventApi.DTO;
using EventApi.Interfaces;

namespace EventApi.Helpers
{
    public class MapHelper : IMapHelper
    {
        public async Task<List<VenueInfo>> MapVenuesToVenueInfoAsync(List<Venue> venues)
        {
            return await Task.Run(() => venues.Select(venue => new VenueInfo
            {
                VenueId = venue.Venueid,
                Name = venue.Name,
                Address = venue.Address,
                City = venue.City,
                Capacity = venue.Capacity
            }).ToList());
        }

        public async Task<List<EventInfo>> MapEventsToEventsInfoAsync(List<Event> events)
        {
            return await Task.Run(() => events.Select(eventItem => new EventInfo
            {
                EventId = eventItem.Eventid,
                Name = eventItem.Name,
                Description = eventItem.Description,
                EventDateTime = eventItem.Eventdatetime,
                EventStatusName = eventItem.Eventstatus.Eventstatusname,
                Eventstatusid = eventItem.Eventstatus.Eventstatusid
            }).ToList());
        }
    }
}
