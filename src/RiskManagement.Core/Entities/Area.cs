namespace RiskManagement.Core.Entities;

public class Area
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ParentAreaId { get; set; }
    public ICollection<Risk> Risks { get; set; } = new List<Risk>();
    public ICollection<RiskCategory> Categories { get; set; } = new List<RiskCategory>();
}
