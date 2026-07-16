using Microsoft.AspNetCore.Mvc;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;

namespace RiskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActionPlansController : ControllerBase
{
    private readonly IActionPlanRepository _actionPlanRepo;
    private readonly IAuditService _auditService;

    public ActionPlansController(IActionPlanRepository actionPlanRepo, IAuditService auditService)
    {
        _actionPlanRepo = actionPlanRepo;
        _auditService = auditService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _actionPlanRepo.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var plan = await _actionPlanRepo.GetByIdAsync(id);
        return plan is null ? NotFound() : Ok(plan);
    }

    [HttpGet("risk/{riskId:guid}")]
    public async Task<IActionResult> GetByRisk(Guid riskId) =>
        Ok(await _actionPlanRepo.GetByRiskAsync(riskId));

    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdue() =>
        Ok(await _actionPlanRepo.GetOverdueAsync());

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(int status) =>
        Ok(await _actionPlanRepo.GetByStatusAsync((ActionPlanStatus)status));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ActionPlan plan)
    {
        plan.Id = Guid.NewGuid();
        plan.CreatedAt = DateTime.UtcNow;
        plan.UpdatedAt = DateTime.UtcNow;
        plan.CreatedBy = User.Identity?.Name ?? "system";
        var created = await _actionPlanRepo.AddAsync(plan);
        await _auditService.LogAsync("ActionPlan", created.Id, "Created",
            User.Identity?.Name ?? "system", User.Identity?.Name,
            newValues: new { created.Action, created.RiskId, created.CommitmentDate });
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ActionPlan plan)
    {
        var existing = await _actionPlanRepo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        var oldValues = new { existing.Status, existing.Progress, existing.CommitmentDate };

        existing.Action = plan.Action;
        existing.Description = plan.Description;
        existing.Responsible = plan.Responsible;
        existing.CommitmentDate = plan.CommitmentDate;
        existing.Status = plan.Status;
        existing.Progress = plan.Progress;
        existing.Observations = plan.Observations;
        existing.UpdatedAt = DateTime.UtcNow;
        existing.UpdatedBy = User.Identity?.Name;

        await _actionPlanRepo.UpdateAsync(existing);
        await _auditService.LogAsync("ActionPlan", id, "Updated",
            User.Identity?.Name ?? "system", User.Identity?.Name,
            oldValues, new { existing.Status, existing.Progress });
        return Ok(existing);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var plan = await _actionPlanRepo.GetByIdAsync(id);
        if (plan is null) return NotFound();
        await _actionPlanRepo.DeleteAsync(plan);
        await _auditService.LogAsync("ActionPlan", id, "Deleted",
            User.Identity?.Name ?? "system", User.Identity?.Name);
        return NoContent();
    }
}
