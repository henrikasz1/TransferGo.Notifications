namespace TransferGo.Notifications.Infrastructure.Services.Sms;

using Microsoft.Extensions.Options;
using TransferGo.Notifications.Application.Services.Sms;
using Options;
using Vonage;
using Vonage.Messaging;
using Vonage.Request;

public class VonageSmsService : ISmsService
{
    private readonly VonageOptions _vonageOptions;
    private readonly VonageClient _client;
    
    public VonageSmsService(IOptions<VonageOptions> vonageSettings)
    {
        _vonageOptions = vonageSettings.Value;
     
        var credentials = Credentials.FromApiKeyAndSecret(
            _vonageOptions.ApiKey,
            _vonageOptions.ApiSecret);

        _client = new VonageClient(credentials);
    }

    public string Name => "Vonage";

    public async Task SendSms(string phoneNumber, string message)
    {
        var request = new SendSmsRequest
        {
            To = phoneNumber,
            From = _vonageOptions.FromPhoneNumber,
            Text = message
        };

        var response = await _client.SmsClient.SendAnSmsAsync(request);

        if (response.Messages[0].Status != "0")
        {
            throw new Exception($"Failed to send SMS: {response.Messages[0].ErrorText}");
        }
    }
}