using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditLogsController : ControllerBase
{
    private readonly RiskDbContext _context;

    public AuditLogsController(RiskDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int limit = 100)
    {
        var logs = await _context.AuditLogs
            .OrderByDescending(l => l.Timestamp)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync();
        return Ok(logs);
    }

    [HttpGet("{entityType}/{entityId:guid}")]
    public async Task<IActionResult> GetByEntity(string entityType, Guid entityId)
    {
        var logs = await _context.AuditLogs
            .Where(l => l.EntityType == entityType && l.EntityId == entityId)
            .OrderByDescending(l => l.Timestamp)
            .AsNoTracking()
            .ToListAsync();
        return Ok(logs);
    }
}
