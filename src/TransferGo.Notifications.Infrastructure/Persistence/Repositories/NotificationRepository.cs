namespace TransferGo.Notifications.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using TransferGo.Notifications.Application.Repositories;
using Domain.Entities;
using Domain.Enums;

public class NotificationRepository : INotificationRepository
{
    private readonly DatabaseContext _databaseContext;
    
    public NotificationRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task CreateNotification(Notification notification)
    {
        await _databaseContext.AddAsync(notification);
        await _databaseContext.SaveChangesAsync();
    }
    
    public async Task UpdateNotification(Notification notification)
    {
        _databaseContext.Notifications.Update(notification);
        await _databaseContext.SaveChangesAsync();
    }

    public async Task<List<Notification>> GetUserNotifications(Guid userId)
    {
        var result = await _databaseContext.Notifications
            .Where(x => x.UserId == userId)
            .ToListAsync();

        return result;
    }
    
    public async Task<Notification> GetNotificationById(Guid notificationId)
    {
        var result = await _databaseContext.Notifications.FirstOrDefaultAsync(x => x.Id == notificationId);

        return result;
    }
    
    public async Task<List<Notification>> GetFailedNotifications()
    {
        var result = await _databaseContext.Notifications
            .Where(x => x.State == State.Error)
            .ToListAsync();

        return result;
    }
}