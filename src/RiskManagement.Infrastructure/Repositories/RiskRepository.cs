using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;

namespace RiskManagement.Infrastructure.Repositories;

public class RiskRepository : IRiskRepository
{
    private readonly RiskDbContext _context;

    public RiskRepository(RiskDbContext context) => _context = context;

    public async Task<Risk?> GetByIdAsync(Guid id) =>
        await _context.Risks.FindAsync(id);

    public async Task<IReadOnlyList<Risk>> GetAllAsync() =>
        await _context.Risks.AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<Risk>> GetByAreaAsync(Guid areaId) =>
        await _context.Risks.Where(r => r.AreaId == areaId).AsNoTracking().ToListAsync();

    public async Task<IReadOnlyList<Risk>> GetByOwnerAsync(string ownerId) =>
        await _context.Risks.Where(r => r.RiskOwnerId == ownerId).AsNoTracking().ToListAsync();

    public async Task<Risk> AddAsync(Risk risk)
    {
        await _context.Risks.AddAsync(risk);
        await _context.SaveChangesAsync();
        return risk;
    }

    public async Task UpdateAsync(Risk risk)
    {
        _context.Risks.Update(risk);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Risk risk)
    {
        _context.Risks.Remove(risk);
        await _context.SaveChangesAsync();
    }
}
