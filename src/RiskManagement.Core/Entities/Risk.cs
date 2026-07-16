using System.Text.Json;

namespace RiskManagement.Core.Entities;

public class Risk
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid AreaId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid RiskOwnerId { get; set; }
    public int Probability { get; set; }
    public int Impact { get; set; }
    public int RiskScore => Probability * Impact;
    public RiskLevel Level => RiskScore switch
    {
        >= 20 => RiskLevel.Critical,
        >= 15 => RiskLevel.High,
        >= 10 => RiskLevel.Medium,
        >= 5 => RiskLevel.Low,
        _ => RiskLevel.VeryLow
    };
    public JsonDocument? DynamicResponses { get; set; }
    public RiskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastReviewDate { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    public ICollection<RiskControl> RiskControls { get; set; } = new List<RiskControl>();
    public ICollection<ActionPlan> ActionPlans { get; set; } = new List<ActionPlan>();
    public ICollection<Evidence> Evidences { get; set; } = new List<Evidence>();
    public ICollection<RiskAuditRequirement> AuditRequirements { get; set; } = new List<RiskAuditRequirement>();
}
