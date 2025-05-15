using MediatR;

namespace TranferGo.Notifications.Application.Queries.GetUserNotifications.Contracts;

public class GetUserNotificationsQuery : IRequest<List<UserNotification>>
{
    public Guid UserId { get; set; }
}