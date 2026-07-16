namespace RiskManagement.Core.Entities;

public class ActionPlan
{
    public Guid Id { get; set; }
    public Guid RiskId { get; set; }
    public Risk Risk { get; set; } = null!;
    public string Action { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Responsible { get; set; } = string.Empty;
    public DateTime CommitmentDate { get; set; }
    public ActionPlanStatus Status { get; set; }
    public int Progress { get; set; }
    public string? Observations { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
}

public enum ActionPlanStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3
}
