namespace TransferGo.Notifications.Application.Queries.GetUserNotifications.Contracts;

using MediatR;

public class GetUserNotificationsQuery : IRequest<List<UserNotification>>
{
    public Guid UserId { get; set; }
}