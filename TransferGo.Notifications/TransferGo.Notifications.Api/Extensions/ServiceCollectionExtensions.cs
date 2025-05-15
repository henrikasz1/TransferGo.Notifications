using Hangfire;
using Microsoft.EntityFrameworkCore;
using TranferGo.Notifications.Application.Commands.SendNotification;
using TranferGo.Notifications.Application.Mappings;
using TranferGo.Notifications.Application.Repositories;
using TranferGo.Notifications.Application.Services.Email;
using TranferGo.Notifications.Application.Services.Notification;
using TranferGo.Notifications.Application.Services.Options;
using TranferGo.Notifications.Application.Services.Sms;
using TranferGo.Notifications.Infrastructure.Persistence;
using TranferGo.Notifications.Infrastructure.Persistence.Repositories;
using TranferGo.Notifications.Infrastructure.Services.Email;
using TranferGo.Notifications.Infrastructure.Services.Options;
using TranferGo.Notifications.Infrastructure.Services.Sms;

namespace TransferGo.Notifications.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDependencies(this IServiceCollection services, IConfigurationRoot configurationRoot)
    {
        services
            .AddDatabase(configurationRoot)
            .AddServices(configurationRoot);
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfigurationRoot configurationRoot)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(configurationRoot.GetConnectionString("DefaultConnection")));

        services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services, IConfigurationRoot configurationRoot)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SendNotificationCommandHandler).Assembly));
        services.AddAutoMapper(typeof(UserNotificationsMappingProfile));
        
        services.AddHangfire(config => config.UseSqlServerStorage(configurationRoot.GetConnectionString("DefaultConnection")));
        services.AddHangfireServer();
        
        services.Configure<TwilioOptions>(configurationRoot.GetSection("NotificationProviders:Twilio"));
        services.Configure<SendGridOptions>(configurationRoot.GetSection("NotificationProviders:SendGrid"));
        services.Configure<VonageOptions>(configurationRoot.GetSection("NotificationProviders:Vonage"));
        services.Configure<MailgunOptions>(configurationRoot.GetSection("NotificationProviders:Mailgun"));
        
        services.Configure<NotificationOptions>(configurationRoot.GetSection("Channels"));

        services.AddScoped<INotificationService, NotificationService>();
        
        services.AddScoped<ISmsService, TwilioSmsService>();
        services.AddScoped<ISmsService, VonageSmsService>();
        
        services.AddScoped<IEmailService, SendGridEmailService>();
        services.AddScoped<IEmailService, MailgunEmailService>();
        
        return services;
    }
}