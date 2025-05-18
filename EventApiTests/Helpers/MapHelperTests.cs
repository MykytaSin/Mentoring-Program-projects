using DAL.Models;
using EventApi.DTO;
using EventApi.Helpers;
using FluentAssertions;

namespace EventApiTests.Helpers
{
    public class MapHelperTests
    {
        private readonly MapHelper _mapHelper;

        public MapHelperTests()
        {
            _mapHelper = new MapHelper();
        }

        [Fact]
        public async Task MapVenuesToVenueInfoAsync_ShouldMapCorrectly()
        {
            // Arrange
            var venues = new List<Venue>
            {
                new Venue { Venueid = 1, Name = "Venue 1", Address = "Address 1", City = "City 1", Capacity = 1000 },
                new Venue { Venueid = 2, Name = "Venue 2", Address = "Address 2", City = "City 2", Capacity = 2000 }
            };

            // Act
            var result = await _mapHelper.MapVenuesToVenueInfoAsync(venues);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(info => info.Name == "Venue 1");
            result.Should().Contain(info => info.City == "City 2");

            var expectedVenueInfos = new List<VenueInfo>
            {
                new VenueInfo { Name = "Venue 1", City = "City 1" },
                new VenueInfo { Name = "Venue 2", City = "City 2" }
            };
            result.Should().BeEquivalentTo(expectedVenueInfos, options => options
                .Excluding(info => info.VenueId)
                .Excluding(info => info.Address)
                .Excluding(info => info.Capacity));
        }

        [Fact]
        public async Task MapVenuesToVenueInfoAsync_ShouldHandleEmptyList()
        {
            // Arrange
            var venues = new List<Venue>();

            // Act
            var result = await _mapHelper.MapVenuesToVenueInfoAsync(venues);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
            result.Should().BeAssignableTo<List<VenueInfo>>();
        }

        [Fact]
        public async Task MapEventsToEventsInfoAsync_ShouldMapCorrectly()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event
                {
                    Eventid = 1,
                    Name = "Event 1",
                    Description = "Description 1",
                    Eventdatetime = DateTime.Now,
                    Eventstatus = new Eventstatus { Eventstatusid = 1, Eventstatusname = "Active" }
                },
                new Event
                {
                    Eventid = 2,
                    Name = "Event 2",
                    Description = "Description 2",
                    Eventdatetime = DateTime.Now.AddDays(1),
                    Eventstatus = new Eventstatus { Eventstatusid = 2, Eventstatusname = "Inactive" }
                }
            };

            // Act
            var result = await _mapHelper.MapEventsToEventsInfoAsync(events);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Event 1", result[0].Name);
            Assert.Equal("Inactive", result[1].EventStatusName);
        }

        [Fact]
        public async Task MapEventsToEventsInfoAsync_ShouldHandleEmptyList()
        {
            // Arrange
            var events = new List<Event>();

            // Act
            var result = await _mapHelper.MapEventsToEventsInfoAsync(events);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void MapPurchaseToOrderInfo_ShouldMapCorrectly()
        {
            // Arrange
            var purchase = new Purchase
            {
                Purchaseid = Guid.NewGuid(),
                Tickets = new List<Ticket>
                {
                    new Ticket
                    {
                        Ticketid = 1,
                        Event = new Event { Name = "Event 1", Eventdatetime = DateTime.Now },
                        Seat = new Seat { Section = new Section { Sectionname = "Section 1" }, Rownumber = 1, Seatid = 101 },
                        Offerprice = new Offerprice { Price = 50 }
                    },
                    new Ticket
                    {
                        Ticketid = 2,
                        Event = new Event { Name = "Event 2", Eventdatetime = DateTime.Now.AddDays(1) },
                        Seat = new Seat { Section = new Section { Sectionname = "Section 2" }, Rownumber = 2, Seatid = 202 },
                        Offerprice = new Offerprice { Price = 75 }
                    }
                }
            };

            // Act
            var result = _mapHelper.MapPurchaseToOrderInfo(purchase);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(purchase.Purchaseid, result.CartIdentifier);
            Assert.Equal(2, result.MinimizedTickets.Count);
            Assert.Equal(125, result.TotalPrice);
        }

        [Fact]
        public void MapPurchaseToOrderInfo_ShouldHandleEmptyTickets()
        {
            // Arrange
            var purchase = new Purchase
            {
                Purchaseid = Guid.NewGuid(),
                Tickets = new List<Ticket>()
            };

            // Act
            var result = _mapHelper.MapPurchaseToOrderInfo(purchase);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(purchase.Purchaseid, result.CartIdentifier);
            Assert.Empty(result.MinimizedTickets);
            Assert.Equal(0, result.TotalPrice);
        }
    }
}
