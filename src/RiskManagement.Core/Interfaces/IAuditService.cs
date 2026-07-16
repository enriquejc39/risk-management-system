namespace RiskManagement.Core.Interfaces;

public interface IAuditService
{
    Task LogAsync(string entityType, Guid entityId, string action, string userId,
        string? userName = null, object? oldValues = null, object? newValues = null,
        string? ipAddress = null);
}
