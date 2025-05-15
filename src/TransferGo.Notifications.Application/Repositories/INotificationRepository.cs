namespace TransferGo.Notifications.Application.Repositories;

using Domain.Entities;

public interface INotificationRepository
{
    Task CreateNotification(Notification notification);

    Task UpdateNotification(Notification notification);

    Task<List<Notification>> GetUserNotifications(Guid userId);

    Task<Notification> GetNotificationById(Guid notificationId);

    Task<List<Notification>> GetFailedNotifications();
}