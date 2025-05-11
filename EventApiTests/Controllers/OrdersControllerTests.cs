using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventApi.Controllers;
using EventApi.DTO;
using EventApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventApiTests.Controllers
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrdersController _ordersController;

        public OrdersControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _ordersController = new OrdersController(_mockOrderService.Object);
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

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrderInfo = Assert.IsType<OrderInfo>(okResult.Value);
            Assert.Equal(cartId, returnedOrderInfo.CartIdentifier);
        }

        [Fact]
        public void AddSeatToCart_ShouldReturnOkResult_WithUpdatedOrderInfo()
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
            var result = _ordersController.AddSeatToCart(cartId, payload);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrderInfo = Assert.IsType<Task<OrderInfo>>(okResult.Value);
        }

        [Fact]
        public void DeleteSeatFromCart_ShouldReturnOkResult_WithUpdatedOrderInfo()
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
            var result = _ordersController.DeleteSeatFromCart(cartId, eventId, seatId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrderInfo = Assert.IsType<Task<OrderInfo>>(okResult.Value);
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
            var result = _ordersController.BookCart(cartId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }
    }
}
