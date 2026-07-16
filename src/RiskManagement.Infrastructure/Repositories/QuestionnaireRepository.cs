using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Infrastructure.Repositories;

public class QuestionnaireRepository : BaseRepository<Questionnaire>, IQuestionnaireRepository
{
    public QuestionnaireRepository(RiskDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Questionnaire>> GetByAreaAsync(Guid? areaId)
    {
        var query = _context.Questionnaires.AsQueryable();

        if (areaId.HasValue)
            query = query.Where(q => q.AreaId == areaId.Value || q.AreaId == null);
        else
            query = query.Where(q => q.AreaId == null);

        return await query.Include(q => q.Questions.OrderBy(qq => qq.Order))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Questionnaire?> GetFullAsync(Guid id) =>
        await _context.Questionnaires
            .Include(q => q.Questions.OrderBy(qq => qq.Order))
            .Include(q => q.Area)
            .FirstOrDefaultAsync(q => q.Id == id);
}
