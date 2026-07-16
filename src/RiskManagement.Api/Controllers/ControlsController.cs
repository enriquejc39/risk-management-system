using Microsoft.AspNetCore.Mvc;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;

namespace RiskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ControlsController : ControllerBase
{
    private readonly IControlRepository _controlRepo;
    private readonly IAuditService _auditService;

    public ControlsController(IControlRepository controlRepo, IAuditService auditService)
    {
        _controlRepo = controlRepo;
        _auditService = auditService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _controlRepo.GetAllAsync());

    [HttpGet("active")]
    public async Task<IActionResult> GetActive() =>
        Ok(await _controlRepo.GetActiveAsync());

    [HttpGet("area/{area}")]
    public async Task<IActionResult> GetByArea(string area) =>
        Ok(await _controlRepo.GetByAreaAsync(area));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var control = await _controlRepo.GetByIdAsync(id);
        return control is null ? NotFound() : Ok(control);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Control control)
    {
        control.Id = Guid.NewGuid();
        control.CreatedAt = DateTime.UtcNow;
        control.CreatedBy = User.Identity?.Name ?? "system";
        var created = await _controlRepo.AddAsync(control);
        await _auditService.LogAsync("Control", created.Id, "Created",
            User.Identity?.Name ?? "system", User.Identity?.Name,
            newValues: new { created.Name, created.ControlType });
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Control control)
    {
        var existing = await _controlRepo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        existing.Name = control.Name;
        existing.Description = control.Description;
        existing.ControlType = control.ControlType;
        existing.Frequency = control.Frequency;
        existing.ResponsibleArea = control.ResponsibleArea;
        existing.IsActive = control.IsActive;
        await _controlRepo.UpdateAsync(existing);

        await _auditService.LogAsync("Control", id, "Updated",
            User.Identity?.Name ?? "system", User.Identity?.Name);
        return Ok(existing);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var control = await _controlRepo.GetByIdAsync(id);
        if (control is null) return NotFound();
        await _controlRepo.DeleteAsync(control);
        await _auditService.LogAsync("Control", id, "Deleted",
            User.Identity?.Name ?? "system", User.Identity?.Name);
        return NoContent();
    }
}
