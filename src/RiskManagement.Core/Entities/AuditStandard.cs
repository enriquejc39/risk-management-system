namespace RiskManagement.Core.Entities;

public class AuditStandard
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<AuditRequirement> Requirements { get; set; } = new List<AuditRequirement>();
}

public class AuditRequirement
{
    public Guid Id { get; set; }
    public Guid AuditStandardId { get; set; }
    public AuditStandard AuditStandard { get; set; } = null!;
    public string RequirementCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Category { get; set; }
    public ICollection<RiskAuditRequirement> RiskRequirements { get; set; } = new List<RiskAuditRequirement>();
}

public class RiskAuditRequirement
{
    public Guid RiskId { get; set; }
    public Risk Risk { get; set; } = null!;
    public Guid AuditRequirementId { get; set; }
    public AuditRequirement AuditRequirement { get; set; } = null!;
    public string ComplianceStatus { get; set; } = "Pending";
    public string? EvidenceNotes { get; set; }
    public DateTime? LastReviewedAt { get; set; }
    public string? ReviewedBy { get; set; }
}
