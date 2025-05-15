using TranferGo.Notifications.Domain.Entities;

namespace TranferGo.Notifications.Application.Repositories;

public interface INotificationRepository
{
    Task CreateNotification(Notification notification);

    Task UpdateNotification(Notification notification);

    Task<List<Notification>> GetUserNotifications(Guid userId);

    Task<Notification> GetNotificationById(Guid notificationId);

    Task<List<Notification>> GetFailedNotifications();
}