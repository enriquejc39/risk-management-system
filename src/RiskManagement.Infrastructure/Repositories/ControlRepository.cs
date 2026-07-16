using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Infrastructure.Repositories;

public class ControlRepository : BaseRepository<Control>, IControlRepository
{
    public ControlRepository(RiskDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Control>> GetByAreaAsync(string responsibleArea) =>
        await _context.Controls
            .Where(c => c.ResponsibleArea == responsibleArea)
            .AsNoTracking()
            .ToListAsync();

    public async Task<IReadOnlyList<Control>> GetActiveAsync() =>
        await _context.Controls
            .Where(c => c.IsActive)
            .AsNoTracking()
            .ToListAsync();
}
