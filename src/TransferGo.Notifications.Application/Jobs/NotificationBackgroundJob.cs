namespace TransferGo.Notifications.Application.Jobs;

using Repositories;
using Services.Notification;
using Hangfire;

public class NotificationBackgroundJob
{
    private readonly INotificationService _notificationService;
    private readonly INotificationRepository _notificationRepository;

    public NotificationBackgroundJob(INotificationService notificationService, INotificationRepository notificationRepository)
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