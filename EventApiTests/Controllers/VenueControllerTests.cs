using DAL.Models;
using EventApi.Controllers;
using EventApi.DTO;
using EventApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventApiTests.Controllers
{
    public class VenueControllerTests
    {
        private readonly Mock<IVenueService> _mockVenueService;
        private readonly VenueController _venueController;

        public VenueControllerTests()
        {
            _mockVenueService = new Mock<IVenueService>();
            _venueController = new VenueController(_mockVenueService.Object);
        }

        [Fact]
        public async Task GetAllVenues_ShouldReturnOkResult_WithListOfVenues()
        {
            // Arrange
            var mockVenues = new List<VenueInfo>
            {
                new VenueInfo { VenueId = 1, Name = "Venue A", Address = "123 Street", City = "City A", Capacity = 1000 },
                new VenueInfo { VenueId = 2, Name = "Venue B", Address = "456 Avenue", City = "City B", Capacity = 2000 }
            };
            _mockVenueService.Setup(service => service.GetAllVenues())
                .ReturnsAsync(mockVenues);

            // Act
            var result = await _venueController.GetAllVenues();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedVenues = Assert.IsType<List<VenueInfo>>(okResult.Value);
            Assert.Equal(mockVenues.Count, returnedVenues.Count);
        }

        [Fact]
        public async Task GetAllVenueSections_ShouldReturnOkResult_WithListOfSections()
        {
            // Arrange
            var venueId = 1;
            var mockSections = new List<Section>
            {
                new Section { Sectionid = 1, Sectionname = "Section A" },
                new Section { Sectionid = 2, Sectionname = "Section B" }
            };
            _mockVenueService.Setup(service => service.GetAllVenuesSection(venueId))
                .ReturnsAsync(mockSections);

            // Act
            var result = await _venueController.GetAllVenueSections(venueId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSections = Assert.IsType<List<Section>>(okResult.Value);
            Assert.Equal(mockSections.Count, returnedSections.Count);
        }
    }
}
