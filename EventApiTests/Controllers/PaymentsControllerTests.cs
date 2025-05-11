using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventApi.Controllers;
using EventApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventApiTests.Controllers
{
    public class PaymentsControllerTests
    {
        private readonly Mock<IPaymentService> _mockPaymentService;
        private readonly PaymentsController _paymentsController;

        public PaymentsControllerTests()
        {
            _mockPaymentService = new Mock<IPaymentService>();
            _paymentsController = new PaymentsController(_mockPaymentService.Object);
        }

        [Fact]
        public async Task GetPaymentStatus_ShouldReturnOkResult_WithPaymentStatus()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var mockStatus = "Completed";
            _mockPaymentService.Setup(service => service.GetPaymentStatus(paymentId))
                .ReturnsAsync(mockStatus);

            // Act
            var result = await _paymentsController.GetPaymentStatus(paymentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStatus = Assert.IsType<string>(okResult.Value);
            Assert.Equal(mockStatus, returnedStatus);
        }

        [Fact]
        public async Task CompletePayment_ShouldReturnOkResult_WithCompletionStatus()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var mockCompletionStatus = true;
            _mockPaymentService.Setup(service => service.CompletePayment(paymentId))
                .ReturnsAsync(mockCompletionStatus);

            // Act
            var result = await _paymentsController.CompletePayment(paymentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStatus = Assert.IsType<bool>(okResult.Value);
            Assert.True(returnedStatus);
        }

        [Fact]
        public async Task RollbackPayment_ShouldReturnOkResult_WithRollbackStatus()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var mockRollbackStatus = true;
            _mockPaymentService.Setup(service => service.RollBackPayment(paymentId))
                .ReturnsAsync(mockRollbackStatus);

            // Act
            var result = await _paymentsController.RollbackPayment(paymentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStatus = Assert.IsType<bool>(okResult.Value);
            Assert.True(returnedStatus);
        }
    }
}
