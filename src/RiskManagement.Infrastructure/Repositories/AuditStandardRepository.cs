using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Infrastructure.Repositories;

public class AuditStandardRepository : BaseRepository<AuditStandard>, IAuditStandardRepository
{
    public AuditStandardRepository(RiskDbContext context) : base(context) { }

    public async Task<AuditStandard?> GetByCodeAsync(string code) =>
        await _context.AuditStandards
            .Include(s => s.Requirements)
            .FirstOrDefaultAsync(s => s.Code == code);

    public async Task<IReadOnlyList<AuditRequirement>> GetRequirementsAsync(Guid standardId) =>
        await _context.AuditRequirements
            .Where(r => r.AuditStandardId == standardId)
            .AsNoTracking()
            .ToListAsync();
}
