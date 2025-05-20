using DAL.Models;
using EventApi.Controllers;
using EventApi.DTO;
using EventApi.Helpers;
using EventApi.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventApiTests.Controllers
{
    public class EventControllerTests
    {
        private readonly Mock<IEventService> _mockEventService;
        private readonly EventController _eventController;

        public EventControllerTests()
        {
            _mockEventService = new Mock<IEventService>();
            _eventController = new EventController(_mockEventService.Object);
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnOkResult_WithListOfEvents()
        {
            // Arrange
            var mockEvents = new List<EventInfo>
            {
                new EventInfo { EventId = 1, Name = "Event 1", EventDateTime = DateTime.Now },
                new EventInfo { EventId = 2, Name = "Event 2", EventDateTime = DateTime.Now.AddDays(1) }
            };
            _mockEventService.Setup(service => service.GetAllMinimizedEventInfo())
                .ReturnsAsync(mockEvents);

            // Act
            var result = await _eventController.GetAllEvents();
            var okResult = result.As<OkObjectResult>();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeAssignableTo<IEnumerable<EventInfo>>()
                .And.BeEquivalentTo(mockEvents);
        }

        [Fact]
        public async Task GetSeatsWithStatusAndPriceOptions_ShouldReturnOkResult_WithListOfSeats()
        {
            // Arrange
            int eventId = 1, sectionId = 1;
            var mockSeats = new List<SeatsWithStatusAndPrice>
            {
                new SeatsWithStatusAndPrice
                {
                    SectionId = sectionId,
                    RowNumber = 1,
                    SeatId = 101,
                    Status = new Ticketstatus { Ticketstatusname = Constants.TicketStatusAvailable },
                    PriceOptions = new List<MinimizedPriceOprions>
                    {
                        new MinimizedPriceOprions { Price = 100.0m },
                        new MinimizedPriceOprions { Price = 150.0m }
                    }
                }
            };
            _mockEventService.Setup(service => service.GetSeatsWithStatusAndPriceOptions(eventId, sectionId))
                .ReturnsAsync(mockSeats);

            // Act
            var result = await _eventController.GetSeatsWithStatusAndPriceOptions(eventId, sectionId);
            var okResult = result.As<OkObjectResult>();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeAssignableTo<IEnumerable<SeatsWithStatusAndPrice>>()
                .And.BeEquivalentTo(mockSeats);
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnEmptyList_WhenNoEventsExist()
        {
            // Arrange
            _mockEventService.Setup(service => service.GetAllMinimizedEventInfo())
                .ReturnsAsync(new List<EventInfo>());

            // Act
            var result = await _eventController.GetAllEvents();
            var okResult = result.As<OkObjectResult>();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            okResult.Value.Should().BeAssignableTo<IEnumerable<EventInfo>>()
                .And.BeEquivalentTo(new List<EventInfo>());

        }

        [Fact]
        public async Task GetSeatsWithStatusAndPriceOptions_ShouldReturnEmptyList_WhenNoSeatsExist()
        {
            // Arrange
            int eventId = 1, sectionId = 1;
            _mockEventService.Setup(service => service.GetSeatsWithStatusAndPriceOptions(eventId, sectionId))
                .ReturnsAsync(new List<SeatsWithStatusAndPrice>());

            // Act
            var result = await _eventController.GetSeatsWithStatusAndPriceOptions(eventId, sectionId);
            var okResult = result.As<OkObjectResult>();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            okResult.Value.Should().BeAssignableTo<IEnumerable<SeatsWithStatusAndPrice>>()
                .And.BeEquivalentTo(new List<SeatsWithStatusAndPrice>());
        }
    }
}
