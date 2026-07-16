namespace RiskManagement.Core.Entities;

public class Control
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ControlType { get; set; } = "Preventivo";
    public string Frequency { get; set; } = "Diario";
    public string ResponsibleArea { get; set; } = string.Empty;
    public string? EvidencePath { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public ICollection<RiskControl> RiskControls { get; set; } = new List<RiskControl>();
}

public class RiskControl
{
    public Guid RiskId { get; set; }
    public Risk Risk { get; set; } = null!;
    public Guid ControlId { get; set; }
    public Control Control { get; set; } = null!;
    public string? Effectiveness { get; set; }
    public DateTime? LastReviewDate { get; set; }
}
