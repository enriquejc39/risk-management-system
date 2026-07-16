namespace RiskManagement.Core.Entities;

public class RiskCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid AreaId { get; set; }
    public Area Area { get; set; } = null!;
    public ICollection<Risk> Risks { get; set; } = new List<Risk>();
}
