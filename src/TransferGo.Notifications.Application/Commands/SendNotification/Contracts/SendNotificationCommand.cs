namespace TransferGo.Notifications.Application.Commands.SendNotification.Contracts;

using Common.Enums;
using MediatR;

public class SendNotificationCommand : IRequest
{
    public Guid UserId { get; set; }
    
    public string EmailAddress { get; set; }

    public string EmailSubject { get; set; }

    public string EmailBody { get; set; }

    public string PhoneNumber { get; set; }

    public string PhoneMessage { get; set; }
    
    public Channel Channel { get; set; }
}