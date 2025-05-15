namespace TransferGo.Notifications.Application.Services.Notification;

using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using Repositories;
using Email;
using Options;
using Sms;
using Domain.Enums;

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
                await Send(
                    notification,
                    _smsProviders,
                    service => service.SendSms(notification.PhoneNumber, notification.PhoneMessage)
                );
                
                break;

            case Channel.Email:
                await Send(
                    notification,
                    _emailProviders,
                    service => service.SendEmail(notification.EmailAddress, notification.EmailSubject, notification.EmailBody)
                );

                break;

            default:
                throw new NotSupportedException($"Unsupported channel: {notification.Channel}");
        }
        
        await _notificationRepository.UpdateNotification(notification);
    }
    
    private async Task Send<TService>(
        Domain.Entities.Notification notification,
        Dictionary<string, TService> serviceProviders,
        Func<TService, Task> sendAction)
    {
        var channelKey = notification.Channel.ToString();

        if (!_notificationOptions.TryGetValue(channelKey, out var channelOptions) || !channelOptions.Enabled)
        {
            notification.SetError();
            await _notificationRepository.UpdateNotification(notification);
            
            throw new InvalidOperationException($"{channelKey} channel is disabled.");
        }

        foreach (var provider in GetActiveProviders(channelOptions))
        {
            try
            {
                if (serviceProviders.TryGetValue(provider.Name, out var service))
                {
                    await _retryPolicy.ExecuteAsync(() => sendAction(service));
                    
                    notification.SetCompleted();
                    
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{channelKey} provider {provider.Name} failed: {ex.Message}");
            }
        }

        notification.SetError();
        
        throw new Exception($"All {channelKey} providers failed.");
    }
    
    private static IEnumerable<ProviderOptions> GetActiveProviders(ChannelOptions channelOptions)
    {
        return channelOptions.Providers
            .Where(p => p.Enabled)
            .OrderBy(p => p.Priority);
    }
}