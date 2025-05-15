using TranferGo.Notifications.Domain.Entities;

namespace TranferGo.Notifications.Application.Repositories;

public interface INotificationsRepository
{
    Task CreateNotification(Notification notification);

    Task UpdateNotification(Notification notification);

    Task<List<Notification>> GetUserNotifications(Guid userId);

    Task<List<Notification>> GetFailedNotifications();
}