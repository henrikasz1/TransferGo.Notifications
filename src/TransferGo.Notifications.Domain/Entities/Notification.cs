using TransferGo.Notifications.Domain.Enums;

namespace TransferGo.Notifications.Domain.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public Channel Channel { get; private set; }
    
    public string EmailAddress { get; private set; }

    public string EmailSubject { get; private set; }

    public string EmailBody { get; private set; }

    public string PhoneNumber { get; private set; }

    public string PhoneMessage { get; set; }

    public State State { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public static Notification Create(
        Guid userId,
        Channel channel,
        string emailAddress,
        string emailSubject,
        string emailBody,
        string phoneNumber,
        string phoneMessage)
    {
        return new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Channel = channel,
            EmailAddress = emailAddress,
            EmailSubject = emailSubject,
            EmailBody = emailBody,
            PhoneNumber = phoneNumber,
            PhoneMessage = phoneMessage,
            State = State.Initialized,
            CreatedAt = DateTimeOffset.Now
        };
    }
    
    public void SetCompleted()
    {
        CompletedAt = DateTimeOffset.Now;
        State = State.Completed;
    }

    public void SetError()
    {
        State = State.Error;
    }
}