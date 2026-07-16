using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Entities;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Api.Controllers;

[ApiController]
[Route("api/catalog")]
public class CatalogController : ControllerBase
{
    private readonly RiskDbContext _context;

    public CatalogController(RiskDbContext context) => _context = context;

    [HttpGet("risks")]
    public async Task<IActionResult> GetRiskCatalog()
    {
        var risks = await _context.Risks
            .Select(r => new
            {
                r.Id,
                r.Name,
                r.Description,
                r.Probability,
                r.Impact,
                r.RiskScore,
                Level = r.Level.ToString(),
                r.Status,
                r.CreatedAt,
                r.UpdatedAt,
                r.LastReviewDate
            })
            .OrderBy(r => r.Name)
            .AsNoTracking()
            .ToListAsync();
        return Ok(risks);
    }

    [HttpGet("areas")]
    public async Task<IActionResult> GetAreas() =>
        Ok(await _context.Areas.AsNoTracking().ToListAsync());

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories() =>
        Ok(await _context.RiskCategories.Include(c => c.Area).AsNoTracking().ToListAsync());

    [HttpGet("categories/{areaId:guid}")]
    public async Task<IActionResult> GetCategoriesByArea(Guid areaId) =>
        Ok(await _context.RiskCategories
            .Where(c => c.AreaId == areaId)
            .AsNoTracking()
            .ToListAsync());

    [HttpGet("methodologies")]
    public async Task<IActionResult> GetMethodologies() =>
        Ok(await _context.Methodologies
            .Include(m => m.Scales)
            .AsNoTracking()
            .ToListAsync());

    [HttpGet("evidences/{riskId:guid}")]
    public async Task<IActionResult> GetEvidences(Guid riskId) =>
        Ok(await _context.Evidences
            .Where(e => e.RiskId == riskId)
            .OrderByDescending(e => e.UploadedAt)
            .AsNoTracking()
            .ToListAsync());

    [HttpPost("evidences")]
    public async Task<IActionResult> UploadEvidence([FromBody] Evidence evidence)
    {
        evidence.Id = Guid.NewGuid();
        evidence.UploadedAt = DateTime.UtcNow;
        evidence.UploadedBy = User.Identity?.Name ?? "system";
        await _context.Evidences.AddAsync(evidence);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvidences), new { riskId = evidence.RiskId }, evidence);
    }

    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications(
        [FromServices] RiskManagement.Core.Interfaces.INotificationService notificationService) =>
        Ok(await notificationService.GetPendingAsync());
}
