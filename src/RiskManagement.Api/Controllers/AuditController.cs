using Microsoft.AspNetCore.Mvc;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;

namespace RiskManagement.Api.Controllers;

[ApiController]
[Route("api/audit")]
public class AuditController : ControllerBase
{
    private readonly IAuditStandardRepository _standardRepo;
    private readonly IRiskRepository _riskRepo;
    private readonly IAuditService _auditService;

    public AuditController(
        IAuditStandardRepository standardRepo,
        IRiskRepository riskRepo,
        IAuditService auditService)
    {
        _standardRepo = standardRepo;
        _riskRepo = riskRepo;
        _auditService = auditService;
    }

    [HttpGet("standards")]
    public async Task<IActionResult> GetStandards() =>
        Ok(await _standardRepo.GetAllAsync());

    [HttpGet("standards/{code}")]
    public async Task<IActionResult> GetStandardByCode(string code)
    {
        var standard = await _standardRepo.GetByCodeAsync(code);
        return standard is null ? NotFound() : Ok(standard);
    }

    [HttpGet("standards/{standardId:guid}/requirements")]
    public async Task<IActionResult> GetRequirements(Guid standardId) =>
        Ok(await _standardRepo.GetRequirementsAsync(standardId));

    [HttpGet("readiness/{standardId:guid}")]
    public async Task<IActionResult> GetReadiness(Guid standardId)
    {
        var standard = await _standardRepo.GetByIdAsync(standardId);
        if (standard is null) return NotFound();

        var requirements = await _standardRepo.GetRequirementsAsync(standardId);
        var riskReqs = new List<object>();

        foreach (var req in requirements)
        {
            var mappedRisks = new List<object>();
            riskReqs.Add(new
            {
                req.RequirementCode,
                req.Description,
                req.Category,
                MappedRisks = mappedRisks
            });
        }

        return Ok(new
        {
            Standard = standard?.Name ?? "",
            TotalRequirements = requirements.Count,
            Requirements = riskReqs
        });
    }

    [HttpPost("risks/{riskId:guid}/requirements/{requirementId:guid}")]
    public async Task<IActionResult> MapRiskToRequirement(
        Guid riskId, Guid requirementId,
        [FromBody] MapRequest request)
    {
        var risk = await _riskRepo.GetByIdAsync(riskId);
        if (risk is null) return NotFound("Riesgo no encontrado");

        await _auditService.LogAsync("RiskAuditMapping", riskId, "Mapped",
            User.Identity?.Name ?? "system", User.Identity?.Name,
            newValues: new { AuditRequirementId = requirementId, request.ComplianceStatus });

        return Ok(new { RiskId = riskId, AuditRequirementId = requirementId, request.ComplianceStatus });
    }

    [HttpPost("generate-report/{standardId:guid}")]
    public async Task<IActionResult> GenerateReport(Guid standardId)
    {
        var standard = await _standardRepo.GetByIdAsync(standardId);
        if (standard is null) return NotFound();

        var report = new
        {
            Standard = standard?.Name ?? "",
            GeneratedAt = DateTime.UtcNow,
            GeneratedBy = User.Identity?.Name ?? "system",
            Summary = "Reporte generado exitosamente"
        };

        return Ok(report);
    }
}

public record MapRequest(string ComplianceStatus, string? EvidenceNotes = null);
