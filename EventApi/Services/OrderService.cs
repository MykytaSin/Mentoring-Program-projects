using DAL.Interfaces;
using DAL.Models;
using EventApi.DTO;
using EventApi.Helpers;
using EventApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MyAppContext _context;
        private readonly IMapHelper _mapHelper;
        private readonly IPaymentService _paymentService;


        public OrderService(IUnitOfWork unitOfWork, IMapHelper mapHelper, MyAppContext context, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapHelper = mapHelper;
            _context = context;
            _paymentService = paymentService;
        }


        public async Task<OrderInfo> GetOrderAsync(Guid cartIdentifier)
        {
            var orders = _unitOfWork.Repository<Purchase>().GetAllAsync().ToList();

            var order = await _context.Purchases.Include(p => p.Tickets).ThenInclude(t => t.Seat).ThenInclude(s => s.Section)
                .Include(p => p.Tickets).ThenInclude(t => t.Offerprice)
                .Include(p => p.Tickets).ThenInclude(t => t.Event)
                .Where(p => string.Equals(p.Purchaseid, cartIdentifier))
                .Where(p => p.Purchasestatus.Purchasestatusname == Constants.PurchaseStatusPending).FirstOrDefaultAsync();

            if (order == null)
            {
                throw new Exception("Order not found");
            }

            return _mapHelper.MapPurchaseToOrderInfo(order);
        }

        public async Task<OrderInfo> AddNewTicketToOrderAsync(Guid cartIdentifier, CartTicketData ticketData)
        {
            //Throws its temporary desiaseon for debuging, Later it will be removed

            Ticket? ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Eventid == ticketData.EventId && t.Seatid == ticketData.SeatId && t.Offerpriceid == ticketData.PriceId);

            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }

            // Find the purchase associated with the cartIdentifier
            var purchase = await _context.Purchases
                .Include(p => p.Tickets)
                .FirstOrDefaultAsync(p => string.Equals(p.Purchaseid, cartIdentifier));

            if (purchase == null)
            {
                throw new Exception("Purchase not found");
            }

            // Add the ticket to the purchase
            purchase.Tickets.Add(ticket);
            // Save changes to the database
            await _context.SaveChangesAsync();

            return await GetOrderAsync(cartIdentifier);
        }

        public async Task<OrderInfo> DeletTicketAsync(Guid cart_id, int event_id, int seat_id)
        {
            //Throws its temporary desiaseon for debuging, Later it will be removed

            Ticket? ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Eventid == event_id && t.Seatid == seat_id);
            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }

            // Find the purchase associated with the cartIdentifier
            var purchase = await _context.Purchases
                .Include(p => p.Tickets)
                .FirstOrDefaultAsync(p => string.Equals(p.Purchaseid, cart_id));

            if (purchase == null)
            {
                throw new Exception("Purchase not found");
            }

            purchase.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return await GetOrderAsync(cart_id);
        }

        public async Task<Guid> MooveTicketsInCartToBookedStatusAsync(Guid cartIdentifier)
        {
            //Throws its temporary desiaseon for debuging, Later it will be removed

            var purchase = await _context.Purchases
                .Include(p => p.Tickets)
                .FirstOrDefaultAsync(p => p.Purchaseid == cartIdentifier);

            Ticketstatus? bookedStatus = await _context.Ticketstatuses.FirstOrDefaultAsync(t => string.Equals(t.Ticketstatusname, Constants.TicketStatusBooked));

            if (purchase == null)
            {
                throw new Exception("Purchase not found");
            }

            // Check if the purchase status is pending
            if (string.Equals(purchase.Purchasestatus.Purchasestatusname , Constants.PurchaseStatusPending) )
            {
                
            }

            // Update the status of all tickets in the purchase to "Booked"
            foreach (var ticket in purchase.Tickets)
            {
                ticket.Ticketstatusid = bookedStatus.Ticketstatusid;
            }


            // Save changes to the database
            await _context.SaveChangesAsync();

            return await _paymentService.GetPaymentId();

        }


    }
}
