namespace TransferGo.Notifications.Extensions;

using Hangfire;
using Microsoft.EntityFrameworkCore;
using Application.Commands.SendNotification;
using Application.Mappings;
using Application.Repositories;
using Application.Services.Email;
using Application.Services.Notification;
using Application.Services.Options;
using Application.Services.Sms;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services.Email;
using Infrastructure.Services.Options;
using Infrastructure.Services.Sms;

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