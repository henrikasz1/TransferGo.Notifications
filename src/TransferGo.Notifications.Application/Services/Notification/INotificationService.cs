namespace TransferGo.Notifications.Application.Services.Notification;

public interface INotificationService
{
    Task SendNotification(Domain.Entities.Notification command);
}