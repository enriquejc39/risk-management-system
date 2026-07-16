using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly RiskDbContext _context;

    public NotificationService(RiskDbContext context) => _context = context;

    public async Task SendNotificationAsync(Notification notification)
    {
        notification.SentAt = DateTime.UtcNow;
        notification.IsSent = true;
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
    }

    public async Task ScheduleNotificationAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
    }

    public async Task ProcessPendingNotificationsAsync()
    {
        var pending = await _context.Notifications
            .Where(n => !n.IsSent
                && (n.ScheduledFor == null || n.ScheduledFor <= DateTime.UtcNow))
            .ToListAsync();

        foreach (var notification in pending)
        {
            notification.IsSent = true;
            notification.SentAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Notification>> GetPendingAsync() =>
        await _context.Notifications
            .Where(n => !n.IsSent)
            .OrderBy(n => n.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
}
