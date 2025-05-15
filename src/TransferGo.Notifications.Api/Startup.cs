using System.Text.Json.Serialization;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using TranferGo.Notifications.Infrastructure.Persistence;
using TransferGo.Notifications.Extensions;

namespace TransferGo.Notifications;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // Add Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
       services.AddDependencies(_configuration as IConfigurationRoot);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseHangfireDashboard();

        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
        if (env.IsDevelopment())
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                dbContext.Database.Migrate(); // Apply migrations only in development
            }
        }
    }
}