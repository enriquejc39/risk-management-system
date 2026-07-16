using RiskManagement.Core.Entities;

namespace RiskManagement.Core.Interfaces;

public interface IControlRepository : IRepository<Control>
{
    Task<IReadOnlyList<Control>> GetByAreaAsync(string responsibleArea);
    Task<IReadOnlyList<Control>> GetActiveAsync();
}
