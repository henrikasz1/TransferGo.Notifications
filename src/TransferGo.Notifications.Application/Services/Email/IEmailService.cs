namespace TransferGo.Notifications.Application.Services.Email;

public interface IEmailService
{
    string Name { get; }

    Task SendEmail(string emailAddress, string subject, string body);
}