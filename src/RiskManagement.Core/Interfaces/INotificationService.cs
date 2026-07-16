using RiskManagement.Core.Entities;

namespace RiskManagement.Core.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(Notification notification);
    Task ScheduleNotificationAsync(Notification notification);
    Task ProcessPendingNotificationsAsync();
    Task<IReadOnlyList<Notification>> GetPendingAsync();
}
