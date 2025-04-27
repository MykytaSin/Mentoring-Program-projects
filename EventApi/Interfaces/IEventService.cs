using EventApi.DTO;

namespace EventApi.Interfaces
{
    public interface IEventService
    {
        public Task<List<EventInfo>> GetAllMinimizedEventInfo();
        public Task<List<object>> GetSeatsWithStatusAndPriceOptions(int eventId, int sectionId);
    }
}
