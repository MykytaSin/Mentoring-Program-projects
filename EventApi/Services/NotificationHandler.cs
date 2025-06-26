using DAL.Models;
using EventApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Services
{
    public class NotificationHandler
    {
        private readonly IServiceBusMessageReceiver _receiver;
        private readonly MyAppContext _context;
        private readonly IEmailProvider _emailProvider;

        public NotificationHandler(IServiceBusMessageReceiver receiver, MyAppContext db, IEmailProvider emailProvider)
        {
            _receiver = receiver;
            _context = db;
            _emailProvider = emailProvider;
        }

        public async Task StartAsync(string queueName, CancellationToken cancellationToken = default)
        {
            await _receiver.StartReceivingAsync<NotificationMessage>(queueName, async (msg, ct) =>
            {
                await UpdateNotificationStatusAsync(msg.TrackingId, "in progress");

                var result = await _emailProvider.SendEmailAsync(msg.CustomerEmail, msg.CustomerName, msg.Content);

                var status = result ? "sent" : "error";
                await UpdateNotificationStatusAsync(msg.TrackingId, status);

                Console.WriteLine(result
                    ? $"[SUCCESS] Email sent to {msg.CustomerEmail}"
                    : $"[ERROR] Failed to send email to {msg.CustomerEmail}");
            }, cancellationToken);
        }

        private async Task UpdateNotificationStatusAsync(Guid trackingId, string status)
        {
            var notification = await _context.Set<NotificationStatus>()
                .FirstOrDefaultAsync(n => n.TrackingId == trackingId);

            if (notification == null)
            {
                notification = new NotificationStatus
                {
                    TrackingId = trackingId,
                    Status = status,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Add(notification);
            }
            else
            {
                notification.Status = status;
                notification.UpdatedAt = DateTime.UtcNow;
                _context.Update(notification);
            }

            await _context.SaveChangesAsync();
        }

    }
}
