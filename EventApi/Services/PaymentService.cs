using DAL.DalCustomExceptions;
using DAL.Models;
using EventApi.DTO;
using EventApi.Helpers;
using EventApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly MyAppContext _context;
        public PaymentService(MyAppContext myAppContext)
        {
            _context = myAppContext;
        }
        public Guid GetPaymentId()
        {
            return Guid.NewGuid();
        }

        public async Task<string> GetPaymentStatus(Guid payment_id)
        {
            var statusName = await _context.Payments.Include(p => p.Paymentstatus).Where(p => p.Paymentid == payment_id).Select(p => p.Paymentstatus.Statusname).FirstOrDefaultAsync();

            if (statusName == null)
            {
                throw new Exception("Ticket not found");
            }

            return statusName;
        }

        public async Task<bool> CompletePayment(Guid paymentId)
        {
            var payment = await _context.Payments.Include(p => p.Paymentstatus).FirstOrDefaultAsync(p => p.Paymentid == paymentId);

            if (payment == null)
            {
                throw new NullReferenceException("Payment not found");
            }

            var isPaymentUpdated = await SetPaymentStatus(payment, Constants.PaymentStatusCompleted);
            var isSeatUpdated = await UpdateSeatStatus(payment, Constants.SeatTypeSold);

            if (isPaymentUpdated && isSeatUpdated)
            {
                return true;
            }
            else
            {
                throw new Exception("Failed to update payment or seat status");
            }
        }

        public async Task<bool> RollBackPayment(Guid payment_id)
        {
            var payment = await _context.Payments.Include(p => p.Paymentstatus).FirstOrDefaultAsync(p => p.Paymentid == payment_id);

            if (payment == null)
            {
                throw new NullReferenceException("Payment not found");
            }

            var isPaymentUpdated = await SetPaymentStatus(payment, Constants.PaymentStatusFailed);
            var isSeatUpdated = await UpdateSeatStatus(payment, Constants.SeatTypeAvailable);

            if (isPaymentUpdated && isSeatUpdated)
            {
                return true;
            }
            else
            {
                throw new Exception("Failed to update payment or seat status");
            }
        }

        public async Task<bool> SetPaymentStatus(Payment payment, string status)
        {

            var completedStatus = await _context.Paymentstatuses.FirstOrDefaultAsync(ps => ps.Statusname == status);
            payment.Paymentstatus.Statusid = completedStatus.Statusid;
            try
            {
                payment.Paymentstatus.Statusid = completedStatus.Statusid;
                _context.SaveChanges();

            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("PaymentStatus is null");
            }

            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            else
            {
                throw new NullValueEntitySearchExceprion("Failed to update seat status");
            }
        }
        public async Task<bool> UpdateSeatStatus(Payment payment, string status)
        {
            try
            {
                var seats = await _context.Seats
                .Include(s => s.Tickets)
                .ThenInclude(t => t.Purchase)
                .ThenInclude(p => p.Payments)
                .Where(s => s.Tickets.Any(t => t.Purchase.Payments.Any(p => p.Paymentid == payment.Paymentid)))
                .ToListAsync();

                var seatType = await _context.Seattypes.FirstOrDefaultAsync(st => st.Seattypename == status);

                foreach (var seat in seats)
                {
                    seat.Seattype.Seattypeid = seatType.Seattypeid;
                }
            }
            catch (NullReferenceException)
            {
                throw;
            }
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            else
            {
                throw new Exception("Failed to update seat status");
            }
        }

    }
}
