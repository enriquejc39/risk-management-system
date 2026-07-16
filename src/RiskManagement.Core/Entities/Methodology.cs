namespace RiskManagement.Core.Entities;

public class Methodology
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Scale> Scales { get; set; } = new List<Scale>();
}

public class Scale
{
    public Guid Id { get; set; }
    public Guid MethodologyId { get; set; }
    public Methodology Methodology { get; set; } = null!;
    public string Dimension { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Value { get; set; }
    public string? Description { get; set; }
}
