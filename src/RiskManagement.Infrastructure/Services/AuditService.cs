using System.Text.Json;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Infrastructure.Services;

public class AuditService : IAuditService
{
    private readonly RiskDbContext _context;

    public AuditService(RiskDbContext context) => _context = context;

    public async Task LogAsync(string entityType, Guid entityId, string action,
        string userId, string? userName = null, object? oldValues = null,
        object? newValues = null, string? ipAddress = null)
    {
        var log = new AuditLog
        {
            Id = Guid.NewGuid(),
            EntityType = entityType,
            EntityId = entityId,
            Action = action,
            UserId = userId,
            UserName = userName,
            OldValues = oldValues is not null ? JsonSerializer.Serialize(oldValues) : null,
            NewValues = newValues is not null ? JsonSerializer.Serialize(newValues) : null,
            Timestamp = DateTime.UtcNow,
            IpAddress = ipAddress
        };

        await _context.AuditLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}
