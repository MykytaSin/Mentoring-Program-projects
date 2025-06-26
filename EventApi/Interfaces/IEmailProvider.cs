namespace EventApi.Interfaces
{
    public interface IEmailProvider
    {
        Task<bool> SendEmailAsync(string toEmail, string toName, string content);
    }
}
