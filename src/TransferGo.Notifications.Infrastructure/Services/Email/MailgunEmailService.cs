namespace TransferGo.Notifications.Infrastructure.Services.Email;

using Options;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using TransferGo.Notifications.Application.Services.Email;

public class MailgunEmailService : IEmailService
{
    private readonly MailgunOptions _mailgunOptions;
    private readonly HttpClient _httpClient;
    
    public MailgunEmailService(IOptions<MailgunOptions> mailgunSettings)
    {
        _mailgunOptions = mailgunSettings.Value;

        var byteArray = System.Text.Encoding.ASCII.GetBytes($"api:{_mailgunOptions.ApiKey}");

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }
    
    public string Name => "Mailgun";

    public async Task SendEmail(string emailAddress, string subject, string body)
    {
        var content = new FormUrlEncodedContent([
            new KeyValuePair<string, string>("from", _mailgunOptions.FromEmail),
            new KeyValuePair<string, string>("to", emailAddress),
            new KeyValuePair<string, string>("subject", subject),
            new KeyValuePair<string, string>("text", body)
        ]);

        var response = await _httpClient.PostAsync($"https://api.mailgun.net/v3/{_mailgunOptions.Domain}/messages", content);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            
            throw new Exception($"Failed to send email: {responseBody}");
        }
    }
}