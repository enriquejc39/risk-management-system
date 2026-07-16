namespace RiskManagement.Core.Enums;

public enum RiskLevel
{
    VeryLow = 1,
    Low = 2,
    Medium = 3,
    High = 4,
    Critical = 5
}

public enum RiskStatus
{
    Draft = 0,
    Submitted = 1,
    UnderReview = 2,
    Approved = 3,
    Rejected = 4,
    Mitigated = 5,
    Closed = 6
}

public enum AuditAction
{
    Created,
    Updated,
    Deleted,
    Submitted,
    Approved,
    Rejected
}
