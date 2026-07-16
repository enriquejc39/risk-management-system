using Microsoft.AspNetCore.Mvc;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;

namespace RiskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RisksController : ControllerBase
{
    private readonly IRiskRepository _riskRepository;
    private readonly IAuditService _auditService;

    public RisksController(IRiskRepository riskRepository, IAuditService auditService)
    {
        _riskRepository = riskRepository;
        _auditService = auditService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _riskRepository.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var risk = await _riskRepository.GetByIdAsync(id);
        return risk is null ? NotFound() : Ok(risk);
    }

    [HttpGet("area/{areaId:guid}")]
    public async Task<IActionResult> GetByArea(Guid areaId) =>
        Ok(await _riskRepository.GetByAreaAsync(areaId));

    [HttpGet("owner/{ownerId}")]
    public async Task<IActionResult> GetByOwner(string ownerId) =>
        Ok(await _riskRepository.GetByOwnerAsync(ownerId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Risk risk)
    {
        risk.CreatedAt = DateTime.UtcNow;
        risk.UpdatedAt = DateTime.UtcNow;
        var created = await _riskRepository.AddAsync(risk);

        await _auditService.LogAsync("Risk", created.Id, "Created",
            User.Identity?.Name ?? "system", User.Identity?.Name,
            newValues: new { created.Name, created.AreaId, created.Probability, created.Impact });

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Risk risk)
    {
        var existing = await _riskRepository.GetByIdAsync(id);
        if (existing is null) return NotFound();

        var oldValues = new { existing.Probability, existing.Impact, existing.Status };

        existing.Name = risk.Name;
        existing.Description = risk.Description;
        existing.AreaId = risk.AreaId;
        existing.CategoryId = risk.CategoryId;
        existing.Probability = risk.Probability;
        existing.Impact = risk.Impact;
        existing.Status = risk.Status;
        existing.DynamicResponses = risk.DynamicResponses;
        existing.UpdatedAt = DateTime.UtcNow;

        await _riskRepository.UpdateAsync(existing);

        await _auditService.LogAsync("Risk", id, "Updated",
            User.Identity?.Name ?? "system", User.Identity?.Name,
            oldValues, new { existing.Probability, existing.Impact, existing.Status });

        return Ok(existing);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var risk = await _riskRepository.GetByIdAsync(id);
        if (risk is null) return NotFound();

        await _riskRepository.DeleteAsync(risk);
        await _auditService.LogAsync("Risk", id, "Deleted",
            User.Identity?.Name ?? "system", User.Identity?.Name);

        return NoContent();
    }
}
