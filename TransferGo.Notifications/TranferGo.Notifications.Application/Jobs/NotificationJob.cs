using Hangfire;
using TranferGo.Notifications.Application.Repositories;
using TranferGo.Notifications.Application.Services.Notification;

namespace TranferGo.Notifications.Application.Jobs;

public class NotificationJob
{
    private readonly INotificationService _notificationService;
    private readonly INotificationRepository _notificationRepository;

    public NotificationJob(INotificationService notificationService, INotificationRepository notificationRepository)
    {
        _notificationService = notificationService;
        _notificationRepository = notificationRepository;
    }
    
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = [10, 30, 60])] // Retries 3 times with delays
    public async Task SendNotificationWithRetry(Guid notificationId)
    {
        var notification = await _notificationRepository.GetNotificationById(notificationId);

        if (notification == null)
        {
            return;
        }

        await _notificationService.SendNotification(notification);
    }
}