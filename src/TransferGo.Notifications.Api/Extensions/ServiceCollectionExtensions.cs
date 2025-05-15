using TransferGo.Notifications.Application.Commands.SendNotification;
using TransferGo.Notifications.Application.Mappings;
using TransferGo.Notifications.Application.Repositories;
using TransferGo.Notifications.Application.Services.Email;
using TransferGo.Notifications.Application.Services.Notification;
using TransferGo.Notifications.Application.Services.Options;
using TransferGo.Notifications.Application.Services.Sms;
using TransferGo.Notifications.Infrastructure.Persistence;
using TransferGo.Notifications.Infrastructure.Persistence.Repositories;
using TransferGo.Notifications.Infrastructure.Services.Email;
using TransferGo.Notifications.Infrastructure.Services.Options;
using TransferGo.Notifications.Infrastructure.Services.Sms;

namespace TransferGo.Notifications.Extensions;

using Hangfire;
using Microsoft.EntityFrameworkCore;

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