using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Infrastructure.Repositories;

public class ActionPlanRepository : BaseRepository<ActionPlan>, IActionPlanRepository
{
    public ActionPlanRepository(RiskDbContext context) : base(context) { }

    public async Task<IReadOnlyList<ActionPlan>> GetByRiskAsync(Guid riskId) =>
        await _context.ActionPlans
            .Where(a => a.RiskId == riskId)
            .OrderBy(a => a.CommitmentDate)
            .AsNoTracking()
            .ToListAsync();

    public async Task<IReadOnlyList<ActionPlan>> GetByResponsibleAsync(string responsible) =>
        await _context.ActionPlans
            .Where(a => a.Responsible == responsible)
            .AsNoTracking()
            .ToListAsync();

    public async Task<IReadOnlyList<ActionPlan>> GetOverdueAsync() =>
        await _context.ActionPlans
            .Where(a => a.CommitmentDate < DateTime.UtcNow
                && a.Status != ActionPlanStatus.Completed
                && a.Status != ActionPlanStatus.Cancelled)
            .AsNoTracking()
            .ToListAsync();

    public async Task<IReadOnlyList<ActionPlan>> GetByStatusAsync(ActionPlanStatus status) =>
        await _context.ActionPlans
            .Where(a => a.Status == status)
            .AsNoTracking()
            .ToListAsync();
}
