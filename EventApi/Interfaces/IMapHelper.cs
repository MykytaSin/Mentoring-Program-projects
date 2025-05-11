using DAL.Models;
using EventApi.DTO;

namespace EventApi.Interfaces
{
    public interface IMapHelper
    {
        public Task<List<VenueInfo>> MapVenuesToVenueInfoAsync(List<Venue> venues);
        public Task<List<EventInfo>> MapEventsToEventsInfoAsync(List<Event> events);
        public OrderInfo MapPurchaseToOrderInfo(Purchase purchase);
    }
}