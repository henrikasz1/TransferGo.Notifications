namespace TranferGo.Notifications.Application.Services.Email;

public interface IEmailServiceProvider
{
    Task SendEmail(string emailAddress, string subject, string body);
}