namespace TransferGo.Notifications.Infrastructure.Services.Sms;

using Microsoft.Extensions.Options;
using TransferGo.Notifications.Application.Services.Sms;
using Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

public class TwilioSmsService : ISmsService
    {
        private readonly TwilioOptions _twilioOptions;
        
        public TwilioSmsService(IOptions<TwilioOptions> twilioSettings)
        {
            _twilioOptions = twilioSettings.Value;
            
            TwilioClient.Init(_twilioOptions.AccountSid, _twilioOptions.AuthToken);
        }
        
        public string Name => "Twilio";

        public async Task SendSms(string phoneNumber, string message)
        {
            await MessageResource.CreateAsync(
                to: new Twilio.Types.PhoneNumber(phoneNumber),
                from: new Twilio.Types.PhoneNumber(_twilioOptions.PhoneNumber),
                body: message);
        }
    }