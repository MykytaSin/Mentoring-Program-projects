using EventApi.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace EventApi.Services
{
    public class SendGridEmailProvider : IEmailProvider
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public SendGridEmailProvider(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"] ?? throw new ArgumentNullException("SendGrid:ApiKey");
            _fromEmail = configuration["SendGrid:FromEmail"] ?? throw new ArgumentNullException("SendGrid:FromEmail");
            _fromName = configuration["SendGrid:FromName"] ?? "Ticketing App";
        }

        public async Task<bool> SendEmailAsync(string toEmail, string toName, string content)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(toEmail, toName);
            var subject = "Notification from Ticketing App";
            var plainTextContent = content;
            var htmlContent = $"<p>{content}</p>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }
    }
}
