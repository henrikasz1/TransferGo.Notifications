namespace TranferGo.Notifications.Infrastructure.Services.Options;

public class MailgunOptions
{
    public string ApiKey { get; set; }
    
    public string Domain { get; set; }
    
    public string FromEmail { get; set; }
}