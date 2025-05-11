using DAL.Models;
using EventApi.DTO;
using EventApi.Helpers;
using EventApi.Interfaces;
using EventApi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace EventApiTests.Services
{
    public class EventServiceTests
    {
        private readonly Mock<IMapHelper> _mapHelperMock;
        private readonly Mock<MyAppContext> _contextMock;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _mapHelperMock = new Mock<IMapHelper>();
            _contextMock = new Mock<MyAppContext>();
            _eventService = new EventService(_mapHelperMock.Object, _contextMock.Object);
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnAllEvents()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Eventid = 1, Name = "Event 1" },
                new Event { Eventid = 2, Name = "Event 2" }
            };

            _contextMock.Setup(c => c.Events).ReturnsDbSet(events);

            // Act
            var result = await _eventService.GetAllEvents();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Event 1", result[0].Name);
            Assert.Equal("Event 2", result[1].Name);
        }

        [Fact]
        public async Task GetAllMinimizedEventInfo_ShouldReturnMappedEventInfo()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Eventid = 1, Name = "Event 1", Eventstatus = new Eventstatus { Eventstatusname = "Active" } },
                new Event { Eventid = 2, Name = "Event 2", Eventstatus = new Eventstatus { Eventstatusname = "Inactive" } }
            };

            var eventInfoList = new List<EventInfo>
            {
                new EventInfo { EventId = 1, Name = "Event 1", EventStatusName = "Active" },
                new EventInfo { EventId = 2, Name = "Event 2", EventStatusName = "Inactive" }
            };

            MoqMapHelperEventsToEventsInfoAsync();

            _contextMock.Setup(c => c.Events).ReturnsDbSet(events);

            // Act
            var result = await _eventService.GetAllMinimizedEventInfo();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Event 1", result[0].Name);
            Assert.Equal("Active", result[0].EventStatusName);
        }

        [Fact]
        public async Task GetSeatsWithStatusAndPriceOptions_ShouldReturnSeatsWithDetails()
        {
            // Arrange
            var seats = new List<Seat>
            {
                new Seat
                {
                    Sectionid = 1,
                    Rownumber = 1,
                    Seatid = 1,
                    Tickets = new List<Ticket>
                    {
                        new Ticket
                        {
                            Eventid = 1,
                            Ticketstatus = new Ticketstatus { Ticketstatusname = Constants.TicketStatusAvailable },
                            Offerprice = new Offerprice
                            {
                                Offer = new Offer
                                {
                                    Offerprices = new List<Offerprice>
                                    {
                                        new Offerprice { Offerpriceid = 1, Pricelevelid = 1, Price = 100 }
                                    }
                                }
                            }
                        }
                    }
                }
            }.AsQueryable();

            _contextMock.Setup(c => c.Seats).ReturnsDbSet(seats);

            // Act
            var result = await _eventService.GetSeatsWithStatusAndPriceOptions(1, 1);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].SectionId);
            Assert.Equal(1, result[0].RowNumber);
            Assert.Equal(1, result[0].SeatId);
            Assert.Equal("Available", result[0].Status.Ticketstatusname);
            Assert.Single(result[0].PriceOptions);
            Assert.Equal(100, result[0].PriceOptions[0].Price);
        }

        private void MoqMapHelperEventsToEventsInfoAsync()
        {
            _mapHelperMock
               .Setup(mapHelper => mapHelper.MapEventsToEventsInfoAsync(It.IsAny<List<Event>>()))
               .ReturnsAsync((List<Event> events) => events.Select(eventItem => new EventInfo
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
