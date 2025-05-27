using EventApi.Controllers;
using EventApi.DTO;
using EventApi.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace EventApiTests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrdersController _ordersController;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly Mock<ICacheHelper> _cacheHelper;


        public OrdersControllerTests()
        {
            _mockMemoryCache = new Mock<IMemoryCache>();
            _cacheHelper = new Mock<ICacheHelper>();

            _mockOrderService = new Mock<IOrderService>();
            _ordersController = new OrdersController(_mockOrderService.Object, _mockMemoryCache.Object, _cacheHelper.Object);
        }

        [Fact]
        public async Task GetCartItems_ShouldReturnOkResult_WithOrderInfo()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var mockOrderInfo = new OrderInfo
            {
                CartIdentifier = cartId,
                MinimizedTickets = new List<TicketMinimizedInfo>(),
                TotalPrice = 100.0m
            };
            _mockOrderService.Setup(service => service.GetOrderAsync(cartId))
                .ReturnsAsync(mockOrderInfo);

            // Act
            var result = await _ordersController.GetCartItems(cartId);
            var okResult = result.As<OkObjectResult>();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeAssignableTo<OrderInfo>()
                .And.BeEquivalentTo(mockOrderInfo);

        }

        [Fact]
        public async Task AddSeatToCart_ShouldReturnOkResult_WithUpdatedOrderInfo()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var payload = new CartTicketData { EventId = 1, SeatId = 101, PriceId = 1 };
            var mockOrderInfo = new OrderInfo
            {
                CartIdentifier = cartId,
                MinimizedTickets = new List<TicketMinimizedInfo>(),
                TotalPrice = 150.0m
            };
            _mockOrderService.Setup(service => service.AddNewTicketToOrderAsync(cartId, payload))
                .ReturnsAsync(mockOrderInfo);

            // Act
            var result = await _ordersController.AddSeatToCart(cartId, payload) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<OrderInfo>()
                .And.BeEquivalentTo(mockOrderInfo);
        }

        [Fact]
        public async Task DeleteSeatFromCart_ShouldReturnOkResult_WithUpdatedOrderInfo()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var eventId = 1;
            var seatId = 101;
            var mockOrderInfo = new OrderInfo
            {
                CartIdentifier = cartId,
                MinimizedTickets = new List<TicketMinimizedInfo>(),
                TotalPrice = 50.0m
            };
            _mockOrderService.Setup(service => service.DeletTicketAsync(cartId, eventId, seatId))
                .ReturnsAsync(mockOrderInfo);

            // Act
            var result = await _ordersController.DeleteSeatFromCart(cartId, eventId, seatId) as OkObjectResult;

            // Assert

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<OrderInfo>()
                .And.BeEquivalentTo(mockOrderInfo);
        }

        [Fact]
        public async Task BookCart_ShouldReturnOkResult_WithPaymentId()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var mockPaymentId = Guid.NewGuid();
            _mockOrderService.Setup(service => service.MooveTicketsInCartToBookedStatusAsync(cartId))
                .ReturnsAsync(mockPaymentId);

            // Act
            var result = await _ordersController.BookCart(cartId) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<Guid>()
                .And.BeEquivalentTo(mockPaymentId);
        }
    }
}
