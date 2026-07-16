using RiskManagement.Core.Entities;

namespace RiskManagement.Core.Interfaces;

public interface IQuestionnaireRepository : IRepository<Questionnaire>
{
    Task<IReadOnlyList<Questionnaire>> GetByAreaAsync(Guid? areaId);
    Task<Questionnaire?> GetFullAsync(Guid id);
}
