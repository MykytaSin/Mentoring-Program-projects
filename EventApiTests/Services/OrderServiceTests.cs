using DAL.DalCustomExceptions;
using DAL.Models;
using EventApi.DTO;
using EventApi.Helpers;
using EventApi.Interfaces;
using EventApi.Services;
using Moq;
using Moq.EntityFrameworkCore;

namespace EventApiTests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<MyAppContext> _mockContext;
        private readonly Mock<IMapHelper> _mockMapHelper;
        private readonly Mock<IPaymentService> _mockPaymentService;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockContext = new Mock<MyAppContext>();
            _mockMapHelper = new Mock<IMapHelper>();
            _mockPaymentService = new Mock<IPaymentService>();
            _orderService = new OrderService(_mockMapHelper.Object, _mockContext.Object, _mockPaymentService.Object);
        }

        [Fact]
        public async Task GetOrderAsync_ShouldReturnOrderInfo_WhenOrderExists()
        {
            // Arrange
            var cartIdentifier = Guid.NewGuid();
            var purchases = new List<Purchase>{new Purchase
                {
                    Purchaseid = cartIdentifier,
                    Purchasestatus = new Purchasestatus { Purchasestatusname = Constants.PurchaseStatusPending }
                }
            };

            _mockContext.Setup(x => x.Purchases).ReturnsDbSet(purchases);

            var expectedOrderInfo = new OrderInfo { CartIdentifier = cartIdentifier };
            _mockMapHelper.Setup(m => m.MapPurchaseToOrderInfo(purchases.First())).Returns(expectedOrderInfo);

            // Act
            var result = await _orderService.GetOrderAsync(cartIdentifier);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cartIdentifier, result.CartIdentifier);
        }

        [Fact]
        public async Task GetOrderAsync_ShouldThrowException_WhenOrderNotFound()
        {
            // Arrange
            var cartIdentifier = Guid.NewGuid();

            _mockContext.Setup(x => x.Purchases).ReturnsDbSet(new List<Purchase>());

            // Act & Assert
            await Assert.ThrowsAsync<NullValueEntitySearchExceprion>(() => _orderService.GetOrderAsync(cartIdentifier));
        }

        [Fact]
        public async Task AddNewTicketToOrderAsync_ShouldAddTicket_WhenTicketAndPurchaseExist()
        {
            // Arrange
            _mockMapHelper.Setup(m => m.MapPurchaseToOrderInfo(It.IsAny<Purchase>()))
                         .Returns(new OrderInfo {CartIdentifier = Guid.NewGuid(), TotalPrice = 10 });
            var cartIdentifier = Guid.NewGuid();
            var ticketData = new CartTicketData { EventId = 1, SeatId = 2, PriceId = 3 };
            var ticket = new List<Ticket> { new Ticket() { Ticketid = 1, Eventid = 1, Seatid = 2, Offerpriceid = 3 } };
            var purchase = new List<Purchase>
            {  GetMockOrder(cartIdentifier) };

            _mockContext.Setup(x => x.Tickets).ReturnsDbSet(ticket);
            _mockContext.Setup(p => p.Purchases).ReturnsDbSet(purchase);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _orderService.AddNewTicketToOrderAsync(cartIdentifier, ticketData);

            // Assert
            Assert.NotNull(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task AddNewTicketToOrderAsync_ShouldThrowException_WhenTicketNotFound()
        {
            // Arrange
            var cartIdentifier = Guid.NewGuid();
            var ticketData = new CartTicketData { EventId = 1, SeatId = 2, PriceId = 3 };

            _mockContext.Setup(c=>c.Tickets).ReturnsDbSet(new List<Ticket> { });

            // Act & Assert
            await Assert.ThrowsAsync<NullValueEntitySearchExceprion>(() => _orderService.AddNewTicketToOrderAsync(cartIdentifier, ticketData));
        }

        [Fact]
        public async Task DeletTicketAsync_ShouldThrowException_WhenTicketNotFound()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var eventId = 1;
            var seatId = 2;

            _mockContext.Setup(c=>c.Tickets).ReturnsDbSet(new List<Ticket> { });

            // Act & Assert
            await Assert.ThrowsAsync<NullValueEntitySearchExceprion>(() => _orderService.DeletTicketAsync(cartId, eventId, seatId));
        }

        [Fact]
        public async Task MooveTicketsInCartToBookedStatusAsync_ShouldUpdateTickets_WhenPurchaseExists()
        {
            // Arrange
            var cartIdentifier = Guid.NewGuid();
            var purchase = new Purchase { Purchaseid = cartIdentifier, Tickets = new List<Ticket> { new Ticket() } };
            var bookedStatus = new Ticketstatus { Ticketstatusid = 1 , Ticketstatusname = Constants.TicketStatusBooked};

            
            _mockContext.Setup(c=>c.Purchases).ReturnsDbSet(new List<Purchase> { purchase });
            _mockContext.Setup(c => c.Ticketstatuses).ReturnsDbSet(new List<Ticketstatus> { bookedStatus });

            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);
            _mockPaymentService.Setup(p => p.GetPaymentId()).Returns(Guid.NewGuid());

            // Act
            var result = await _orderService.MooveTicketsInCartToBookedStatusAsync(cartIdentifier);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task MooveTicketsInCartToBookedStatusAsync_ShouldThrowException_WhenPurchaseNotFound()
        {
            // Arrange
            var cartIdentifier = Guid.NewGuid();
            var ticketStatus = new Ticketstatus { Ticketstatusname = Constants.TicketStatusBooked };

            _mockContext.Setup(c => c.Purchases).ReturnsDbSet(new List<Purchase> {  });
            _mockContext.Setup(ts => ts.Ticketstatuses).ReturnsDbSet(new List<Ticketstatus> { ticketStatus } );
            // Act & Assert
            await Assert.ThrowsAsync<NullValueEntitySearchExceprion>(() => _orderService.MooveTicketsInCartToBookedStatusAsync(cartIdentifier));
        }

        private static Purchase GetMockOrder(Guid cartIdentifier)
        {
            return new Purchase
            {
                Purchaseid = cartIdentifier,
                Purchasestatus = new Purchasestatus
                {
                    Purchasestatusname = Constants.PurchaseStatusPending
                },
                Tickets = new List<Ticket>
                {
                    new Ticket
                    {
                        Ticketcode = "TICKET123",
                        Event = new Event
                        {
                            Name = "Sample Event",
                            Eventstatus = new Eventstatus
                            {
                                Eventstatusname = "Active"
                            },
                            Venue = new Venue
                            {
                                Name = "Sample Venue"
                            }
                        },
                        Seat = new Seat
                        {
                            Section = new Section
                            {
                                Sectionname = "A1"
                            }
                        },
                        Offerprice = new Offerprice
                        {
                            Offer = new Offer
                            {
                                Offername = "Standard"
                            },
                            Pricelevel = new Pricelevel
                            {
                                Levelname = "Level 1"
                            }
                        },
                        Ticketstatus = new Ticketstatus
                        {
                            Ticketstatusname = "Available"
                        }
                    }
                },
                User = new User
                {
                    Username = "testuser",
                    Email = "testuser@example.com"
                }
            };
        }
    }
}
