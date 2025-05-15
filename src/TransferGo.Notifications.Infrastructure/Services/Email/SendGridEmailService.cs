namespace TransferGo.Notifications.Infrastructure.Services.Email;

using Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using TransferGo.Notifications.Application.Services.Email;

public class SendGridEmailService : IEmailService
{
    private const string EmailName = "test";
    
    private readonly SendGridOptions _sendGridOptions;
    
    public SendGridEmailService(IOptions<SendGridOptions> sendGridSettings)
    {
        _sendGridOptions = sendGridSettings.Value;
    }

    public string Name => "SendGrid";

    public async Task SendEmail(string emailAddress, string subject, string body)
    {
       var client = new SendGridClient(_sendGridOptions.ApiKey);
       
       var email = MailHelper.CreateSingleEmail(
           new EmailAddress(_sendGridOptions.FromEmail, EmailName),
           new EmailAddress(emailAddress),
           subject, 
           body, 
           null);
       
       await client.SendEmailAsync(email);
    }
}