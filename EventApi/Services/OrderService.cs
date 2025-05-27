using DAL.DalCustomExceptions;
using DAL.Models;
using EventApi.DTO;
using EventApi.Helpers;
using EventApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly MyAppContext _context;
        private readonly IMapHelper _mapHelper;
        private readonly IPaymentService _paymentService;

        public OrderService(IMapHelper mapHelper, MyAppContext context, IPaymentService paymentService)
        {
            _mapHelper = mapHelper;
            _context = context;
            _paymentService = paymentService;
        }


        public async Task<OrderInfo> GetOrderAsync(Guid cartIdentifier)
        {
            var orders = await _context.Purchases.ToListAsync();

            var order = await _context.Purchases.Include(p => p.Tickets).ThenInclude(t => t.Seat).ThenInclude(s => s.Section)
                .Include(p => p.Tickets).ThenInclude(t => t.Offerprice)
                .Include(p => p.Tickets).ThenInclude(t => t.Event)
                .Where(p => p.Purchaseid == cartIdentifier)
                .Where(p => p.Purchasestatus.Purchasestatusname == Constants.PurchaseStatusPending).FirstOrDefaultAsync();

            if (order is null)
            {
                throw new NullValueEntitySearchExceprion("Order not found");
            }

            return _mapHelper.MapPurchaseToOrderInfo(order);
        }

        public async Task<OrderInfo> AddNewTicketToOrderAsync(Guid cartIdentifier, CartTicketData ticketData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Ticket? ticket = await _context.Tickets
                        .Include(t => t.Ticketstatus) // Include Ticketstatus to check its name
                        .FirstOrDefaultAsync(t => t.Eventid == ticketData.EventId && t.Seatid == ticketData.SeatId && t.Offerpriceid == ticketData.PriceId);
                    if (ticket is null)
                    {
                        throw new NullValueEntitySearchExceprion("Ticket not found");
                    }

                    // Check if the ticket is already booked or sold
                    if (ticket.Ticketstatus?.Ticketstatusname == Constants.TicketStatusBooked ||
                        ticket.Ticketstatus?.Ticketstatusname == Constants.TicketStatusSold)
                    {
                        throw new InvalidOperationException($"Ticket for Event ID: {ticketData.EventId}, Seat ID: {ticketData.SeatId} is already {ticket.Ticketstatus.Ticketstatusname}.");
                    }

                    var purchase = await _context.Purchases
                        .Include(p => p.Tickets)
                        .FirstOrDefaultAsync(p => p.Purchaseid == cartIdentifier);

                    if (purchase is null)
                    {
                        throw new NullValueEntitySearchExceprion("Purchase not found");
                    }

                    if (purchase.Tickets.Any(t => t.Ticketid == ticket.Ticketid))
                    {

                        return await GetOrderAsync(cartIdentifier);
                    }

                    Ticketstatus? pendingStatus = await _context.Ticketstatuses.FirstOrDefaultAsync(ts => ts.Ticketstatusname == Constants.TicketStatusPending);
                    if (pendingStatus != null)
                    {
                        ticket.Ticketstatusid = pendingStatus.Ticketstatusid;
                    }

                    purchase.Tickets.Add(ticket);
                    await _context.SaveChangesAsync();

                    return await GetOrderAsync(cartIdentifier);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw; 
                }
            }
        }

        public async Task<OrderInfo> DeletTicketAsync(Guid cartId, int eventId, int seatId)
        {

            Ticket? ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Eventid == eventId && t.Seatid == seatId);
            if (ticket is null)
            {
                throw new NullValueEntitySearchExceprion("Ticket not found");
            }

            var purchase = await _context.Purchases
                .Include(p => p.Tickets)
                .FirstOrDefaultAsync(p => string.Equals(p.Purchaseid, cartId));

            purchase.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return await GetOrderAsync(cartId);
        }

        public async Task<Guid> MooveTicketsInCartToBookedStatusAsync(Guid cartIdentifier)
        {
            var purchase = await _context.Purchases
                .Include(p => p.Tickets)
                .FirstOrDefaultAsync(p => p.Purchaseid == cartIdentifier);

            Ticketstatus? bookedStatus = await _context.Ticketstatuses.FirstAsync(t => t.Ticketstatusname == Constants.TicketStatusBooked);

            if (purchase is null)
            {
                throw new NullValueEntitySearchExceprion("Purchase not found");
            }

            foreach (var ticket in purchase.Tickets)
            {
                ticket.Ticketstatusid = bookedStatus.Ticketstatusid;
            }
            await _context.SaveChangesAsync();

            return _paymentService.GetPaymentId();
        }
    }
}
