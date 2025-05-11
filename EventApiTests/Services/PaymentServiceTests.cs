using DAL.Models;
using EventApi.Helpers;
using EventApi.Services;
using Moq;
using Moq.EntityFrameworkCore;

namespace EventApiTests.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<MyAppContext> _mockContext;
        private readonly PaymentService _paymentService;

        public PaymentServiceTests()
        {
            _mockContext = new Mock<MyAppContext>();
            _paymentService = new PaymentService(_mockContext.Object);
        }

        [Fact]
        public async Task GetPaymentId_ShouldReturnNewGuid()
        {
            // Act
            var result = _paymentService.GetPaymentId();

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async Task GetPaymentStatus_ShouldReturnStatusName_WhenPaymentExists()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var paymentStatus = new Paymentstatus { Statusname = Constants.PaymentStatusCompleted };
            var payments = new List<Payment>
            {
                new Payment { Paymentid = paymentId, Paymentstatus = paymentStatus }
            };

            _mockContext.Setup(c => c.Payments).ReturnsDbSet(payments);

            // Act
            var result = await _paymentService.GetPaymentStatus(paymentId);

            // Assert
            Assert.Equal(Constants.PaymentStatusCompleted, result);
        }

        [Fact]
        public async Task GetPaymentStatus_ShouldThrowException_WhenPaymentNotFound()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var payments = new List<Payment>();

            _mockContext.Setup(c => c.Payments).ReturnsDbSet(payments);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _paymentService.GetPaymentStatus(paymentId));
        }

        [Fact]
        public async Task CompletePayment_ShouldReturnTrue_WhenPaymentAndSeatUpdated()
        {
            // Arrange
            var paymentId = Guid.NewGuid();

            SetMockedPaymentStatus();
            SetMockedContext(paymentId);

            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _paymentService.CompletePayment(paymentId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RollBackPayment_ShouldReturnTrue_WhenPaymentAndSeatUpdated()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            SetMockedPaymentStatus();
            SetMockedContext(paymentId);

            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _paymentService.RollBackPayment(paymentId);

            // Assert
            Assert.True(result);
        }


        public void SetMockedPaymentStatus()
        {
            var paymentsStatuses = new List<Paymentstatus>
            {
                new Paymentstatus{ Statusid = 1, Statusname = Constants.PaymentStatusCompleted },
                new Paymentstatus{ Statusid = 2, Statusname = Constants.PaymentStatusFailed },
                new Paymentstatus{ Statusid = 3, Statusname = Constants.PaymentStatusPending }
            };
            _mockContext.Setup(s => s.Paymentstatuses).ReturnsDbSet(paymentsStatuses);
        }

        public void SetMockedContext(Guid paymentId)
        {

            // Mock DbSet for Payments  
            var payments = new List<Payment>
            {
                new Payment
                {
                    Paymentid = paymentId,
                    Paymentstatus = new Paymentstatus { Statusid = 1, Statusname = Constants.PaymentStatusCompleted  },
                    Purchaseid = Guid.NewGuid()
                },
                new Payment
                {
                    Paymentid = Guid.NewGuid(),
                    Paymentstatus = new Paymentstatus { Statusid = 2, Statusname = Constants.PaymentStatusFailed },
                    Purchaseid = Guid.NewGuid()
                }
            };

            _mockContext.Setup(c => c.Payments).ReturnsDbSet(payments);


            // Mock DbSet for Seats  
            var seats = new List<Seat>
            {
                new Seat
                {
                    Tickets = new List<Ticket>
                    {
                        new Ticket
                        {
                            Purchase = new Purchase
                            {
                                Payments = new List<Payment> { payments.First() }
                            }
                        }
                    },
                    Seattype = new Seattype { Seattypeid = 1, Seattypename = Constants.SeatTypeAvailable }

                }
            };

            _mockContext.Setup(c => c.Seats).ReturnsDbSet(seats);


            // Mock DbSet for Seattypes  
            var seatTypes = new List<Seattype>
            {
                new Seattype { Seattypeid = 1, Seattypename = Constants.SeatTypeAvailable },
                new Seattype { Seattypeid = 2, Seattypename = Constants.SeatTypeSold }
            };

            _mockContext.Setup(c => c.Seattypes).ReturnsDbSet(seatTypes);

        }
    }
}
