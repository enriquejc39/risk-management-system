using RiskManagement.Core.Entities;

namespace RiskManagement.Core.Interfaces;

public interface IRiskRepository
{
    Task<Risk?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Risk>> GetAllAsync();
    Task<IReadOnlyList<Risk>> GetByAreaAsync(Guid areaId);
    Task<IReadOnlyList<Risk>> GetByOwnerAsync(string ownerId);
    Task<Risk> AddAsync(Risk risk);
    Task UpdateAsync(Risk risk);
    Task DeleteAsync(Risk risk);
}
