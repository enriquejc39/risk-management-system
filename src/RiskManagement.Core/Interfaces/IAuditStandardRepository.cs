using RiskManagement.Core.Entities;

namespace RiskManagement.Core.Interfaces;

public interface IAuditStandardRepository : IRepository<AuditStandard>
{
    Task<AuditStandard?> GetByCodeAsync(string code);
    Task<IReadOnlyList<AuditRequirement>> GetRequirementsAsync(Guid standardId);
}
