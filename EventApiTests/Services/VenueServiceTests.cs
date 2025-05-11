using DAL.Models;
using EventApi.DTO;
using EventApi.Interfaces;
using EventApi.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace EventApiTests.Services
{
    public class VenueServiceTests
    {
        private readonly Mock<IMapHelper> _mapHelperMock;
        private readonly Mock<MyAppContext> _contextMock;
        private readonly VenueService _venueService;

        public VenueServiceTests()
        {
            _mapHelperMock = new Mock<IMapHelper>();
            _contextMock = new Mock<MyAppContext>();
            _venueService = new VenueService(_mapHelperMock.Object, _contextMock.Object);
        }

        [Fact]
        public async Task GetAllVenues_ShouldReturnMappedVenueInfoList()
        {
            // Arrange
            var venues = new List<Venue>
            {
                new Venue { Venueid = 1, Name = "Venue 1", Address = "Address 1", City = "City 1", Capacity = 1000 },
                new Venue { Venueid = 2, Name = "Venue 2", Address = "Address 2", City = "City 2", Capacity = 2000 }
            };

            var venueInfoList = new List<VenueInfo>
            {
                new VenueInfo { VenueId = 1, Name = "Venue 1", Address = "Address 1", City = "City 1", Capacity = 1000 },
                new VenueInfo { VenueId = 2, Name = "Venue 2", Address = "Address 2", City = "City 2", Capacity = 2000 }
            };

            var dbSetMock = new Mock<DbSet<Venue>>();
            dbSetMock.As<IQueryable<Venue>>().Setup(m => m.Provider).Returns(venues.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Venue>>().Setup(m => m.Expression).Returns(venues.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Venue>>().Setup(m => m.ElementType).Returns(venues.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Venue>>().Setup(m => m.GetEnumerator()).Returns(venues.AsQueryable().GetEnumerator());

            _contextMock.Setup(c => c.Venues).ReturnsDbSet(venues);
            _mapHelperMock.Setup(m => m.MapVenuesToVenueInfoAsync(It.IsAny<List<Venue>>()))
                .ReturnsAsync(venueInfoList);

            // Act
            var result = await _venueService.GetAllVenues();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Venue 1", result[0].Name);
            Assert.Equal("Venue 2", result[1].Name);
        }

        [Fact]
        public async Task GetAllVenuesSection_ShouldReturnSectionsForGivenVenueId()
        {
            // Arrange
            var venueId = 1;
            var sections = new List<Section>
            {
                new Section { Sectionid = 1, Sectionname = "Section 1", Manifestid = 1 },
                new Section { Sectionid = 2, Sectionname = "Section 2", Manifestid = 1 }
            };

            var manifests = new List<Manifest>
            {
                new Manifest { Manifestid = 1, Sections = sections }
            };

            var venues = new List<Venue>
            {
                new Venue { Venueid = venueId, Manifests = manifests }
            };

            _contextMock.Setup(c => c.Venues).ReturnsDbSet(venues);

            // Act
            var result = await _venueService.GetAllVenuesSection(venueId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Section 1", result[0].Sectionname);
            Assert.Equal("Section 2", result[1].Sectionname);
        }
    }
}
