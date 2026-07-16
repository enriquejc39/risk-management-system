using Microsoft.AspNetCore.Mvc;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;

namespace RiskManagement.Api.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IRiskRepository _riskRepo;
    private readonly IAuditService _auditService;
    private readonly IAuditStandardRepository _standardRepo;

    public ReportsController(
        IRiskRepository riskRepo,
        IAuditService auditService,
        IAuditStandardRepository standardRepo)
    {
        _riskRepo = riskRepo;
        _auditService = auditService;
        _standardRepo = standardRepo;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(
        [FromServices] IKpiService kpiService) =>
        Ok(await kpiService.GetDashboardAsync());

    [HttpGet("top-risks")]
    public async Task<IActionResult> GetTopRisks(
        [FromServices] IKpiService kpiService,
        [FromQuery] int count = 10) =>
        Ok(await kpiService.GetTopRisksAsync(count));

    [HttpGet("heatmap")]
    public async Task<IActionResult> GetHeatMap()
    {
        var risks = await _riskRepo.GetAllAsync();
        var heatMap = new List<object>();

        for (int p = 1; p <= 5; p++)
        {
            for (int i = 1; i <= 5; i++)
            {
                var count = risks.Count(r => r.Probability == p && r.Impact == i);
                heatMap.Add(new { Probability = p, Impact = i, Count = count, Score = p * i });
            }
        }

        return Ok(heatMap);
    }

    [HttpGet("audit-report/{standardCode}")]
    public async Task<IActionResult> GetAuditReport(string standardCode)
    {
        var standard = await _standardRepo.GetByCodeAsync(standardCode);
        if (standard is null) return NotFound($"Estándar {standardCode} no encontrado");

        var risks = await _riskRepo.GetAllAsync();
        var requirements = await _standardRepo.GetRequirementsAsync(standard.Id);

        return Ok(new
        {
            Standard = standard.Name,
            ReportDate = DateTime.UtcNow,
            TotalRisks = risks.Count,
            TotalRequirements = requirements.Count,
            CriticalRisks = risks.Count(r => r.Level == RiskLevel.Critical),
            GeneratedBy = User.Identity?.Name
        });
    }

    [HttpGet("history/{entityType}/{entityId:guid}")]
    public async Task<IActionResult> GetHistory(
        string entityType, Guid entityId,
        [FromServices] RiskManagement.Infrastructure.Data.RiskDbContext context)
    {
        var logs = await context.AuditLogs
            .Where(l => l.EntityType == entityType && l.EntityId == entityId)
            .OrderByDescending(l => l.Timestamp)
            .AsNoTracking()
            .ToListAsync();
        return Ok(logs);
    }
}
