namespace TranferGo.Notifications.Application.Services.Sms;

public interface ISmsService
{
    string Name { get; }

    Task SendSms(string phoneNumber, string message);
}