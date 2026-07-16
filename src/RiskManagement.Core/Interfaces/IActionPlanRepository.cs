using RiskManagement.Core.Entities;

namespace RiskManagement.Core.Interfaces;

public interface IActionPlanRepository : IRepository<ActionPlan>
{
    Task<IReadOnlyList<ActionPlan>> GetByRiskAsync(Guid riskId);
    Task<IReadOnlyList<ActionPlan>> GetByResponsibleAsync(string responsible);
    Task<IReadOnlyList<ActionPlan>> GetOverdueAsync();
    Task<IReadOnlyList<ActionPlan>> GetByStatusAsync(ActionPlanStatus status);
}
