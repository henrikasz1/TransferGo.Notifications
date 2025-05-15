namespace TranferGo.Notifications.Application.Services.Notification;

using Microsoft.Extensions.Options;
using Repositories;
using Email;
using Options;
using Sms;
using Domain.Enums;
using Polly;
using Polly.Retry;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    
    private readonly NotificationOptions _notificationOptions;
    
    private readonly Dictionary<string, ISmsService> _smsProviders;
    private readonly Dictionary<string, IEmailService> _emailProviders;
    
    private readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: _ => TimeSpan.FromSeconds(2),
            onRetry: (exception, timeSpan, retryCount, _) =>
            {
                Console.WriteLine($"Retry {retryCount} due to: {exception.Message}. Waiting {timeSpan} seconds before next retry.");
            });
    
    public NotificationService(
        INotificationRepository notificationRepository,
        IOptions<NotificationOptions> notificationOptions,
        IEnumerable<ISmsService> smsProviders,
        IEnumerable<IEmailService> emailProviders)
    {
        _notificationRepository = notificationRepository;

        _notificationOptions = notificationOptions.Value;
        
        _smsProviders = smsProviders.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
        _emailProviders = emailProviders.ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
    }
    
    public async Task SendNotification(Domain.Entities.Notification notification)
    {
        switch (notification.Channel)
        {
            case Channel.Sms:
                await SendSms(notification);
                break;

            case Channel.Email:
                await SendEmail(notification);
                break;

            default:
                throw new NotSupportedException($"Unsupported channel: {notification.Channel}");
        }
        
        await _notificationRepository.UpdateNotification(notification);
    }
    
    private async Task SendSms(Domain.Entities.Notification notification)
    {
        if (!_notificationOptions.TryGetValue(notification.Channel.ToString(), out var smsChannel) || !smsChannel.Enabled)
        {
            notification.SetError();
            await _notificationRepository.UpdateNotification(notification);
            
            throw new InvalidOperationException("SMS channel is disabled.");
        }
        
        foreach (var provider in GetActiveProviders(smsChannel))
        {
            try
            {
                if (_smsProviders.TryGetValue(provider.Name, out var service))
                {
                    await _retryPolicy.ExecuteAsync(() => 
                        service.SendSms(notification.PhoneNumber, notification.PhoneMessage));
                    
                    notification.SetCompleted();
                    
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMS provider {provider.Name} failed: {ex.Message}");
            }
        }

        notification.SetError();
        
        throw new Exception("All SMS providers failed.");
    }

    private async Task SendEmail(Domain.Entities.Notification notification)
    {
        if (!_notificationOptions.TryGetValue(notification.Channel.ToString(), out var emailChannel) || !emailChannel.Enabled)
        {
            notification.SetError();
            await _notificationRepository.UpdateNotification(notification);

            throw new InvalidOperationException("Email channel is disabled.");
        }

        foreach (var provider in GetActiveProviders(emailChannel))
        {
            try
            {
                if (_emailProviders.TryGetValue(provider.Name, out var service))
                {
                    await _retryPolicy.ExecuteAsync(() => 
                        service.SendEmail(notification.EmailAddress, notification.EmailSubject, notification.EmailBody));
                    
                    notification.SetCompleted();
                    
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email provider {provider.Name} failed: {ex.Message}");
            }
        }

        notification.SetError();
        
        throw new Exception("All email providers failed.");
    }
    
    private static IEnumerable<ProviderOptions> GetActiveProviders(ChannelOptions channelOptions)
    {
        return channelOptions.Providers
            .Where(p => p.Enabled)
            .OrderBy(p => p.Priority);
    }
}