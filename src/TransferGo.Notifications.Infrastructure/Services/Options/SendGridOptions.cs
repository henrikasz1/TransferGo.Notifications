namespace TransferGo.Notifications.Infrastructure.Services.Options;

public class SendGridOptions
{
    public string ApiKey { get; set; }
    
    public string FromEmail { get; set; }
}