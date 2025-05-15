using TranferGo.Notifications.Application.Common.Enums;

namespace TranferGo.Notifications.Application.Queries.GetUserNotifications.Contracts;

public class UserNotification
{
    public Channel Channel { get; set; }

    public string EmailAddress { get; set; }

    public string EmailSubject { get; set; }

    public string EmailBody { get; set; }

    public string PhoneNumber { get; set; }

    public string PhoneMessage { get; set; }

    public State State { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }
}