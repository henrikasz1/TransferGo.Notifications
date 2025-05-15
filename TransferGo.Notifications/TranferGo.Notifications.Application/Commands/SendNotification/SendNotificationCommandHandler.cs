using Hangfire;
using TranferGo.Notifications.Application.Jobs;

namespace TranferGo.Notifications.Application.Commands.SendNotification;

using MediatR;
using Contracts;
using Repositories;
using Domain.Entities;
using Domain.Enums;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand>
{
    private readonly INotificationRepository _notificationRepository;
    
    public SendNotificationCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
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
        
        BackgroundJob.Enqueue<NotificationJob>(job => job.SendNotificationWithRetry(notification.Id));
    }
}