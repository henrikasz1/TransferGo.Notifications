namespace TransferGo.Notifications.Application.Commands.SendNotification;

using Hangfire;
using MediatR;
using Contracts;
using Jobs;
using Repositories;
using Domain.Entities;
using Domain.Enums;


public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IBackgroundJobClient _backgroundJobClient;
    
    public SendNotificationCommandHandler(
        INotificationRepository notificationRepository,
        IBackgroundJobClient backgroundJobClient)
    {
        _notificationRepository = notificationRepository;
        _backgroundJobClient = backgroundJobClient;
    }

    public async Task Handle(SendNotificationCommand command, CancellationToken cancellationToken)
    {
        var notification = Notification.Create(
            command.UserId,
            (Channel)command.Channel,
            command.EmailAddress,
            command.EmailSubject,
            command.EmailBody,
            command.PhoneNumber,
            command.PhoneMessage);
        
        await _notificationRepository.CreateNotification(notification);
        
        _backgroundJobClient.Enqueue<NotificationBackgroundJob>(job => job.SendNotificationWithRetry(notification.Id));
    }
}