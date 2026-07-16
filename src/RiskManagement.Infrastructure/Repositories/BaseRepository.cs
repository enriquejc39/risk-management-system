using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Interfaces;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly RiskDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(RiskDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync() =>
        await _dbSet.AsNoTracking().ToListAsync();

    public virtual async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.Where(predicate).AsNoTracking().ToListAsync();

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
