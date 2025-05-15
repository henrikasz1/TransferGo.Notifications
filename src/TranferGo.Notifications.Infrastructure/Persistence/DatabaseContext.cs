using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TranferGo.Notifications.Domain.Enums;

namespace TranferGo.Notifications.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }

    public DbSet<Notification> Notifications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Optional example
        modelBuilder.Entity<Notification>(x =>
        {
            var stateEnumConverter = new ValueConverter<State, string>(
                y => y.ToString(),
                y => (State)Enum.Parse(typeof(State), y));
        
            var channelEnumConverter = new ValueConverter<Channel, string>(
                y => y.ToString(),
                y => (Channel)Enum.Parse(typeof(Channel), y));
            
            x.HasKey(y => y.Id);
            
            x.Property(y => y.UserId).IsRequired();
            x.Property(y => y.Channel).IsRequired().HasConversion(channelEnumConverter);
            x.Property(y => y.State).IsRequired().HasConversion(stateEnumConverter);

            x.HasIndex(y => y.State);
            x.HasIndex(y => y.UserId);
        });
    }
}