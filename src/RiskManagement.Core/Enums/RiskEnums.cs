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

public enum ImpactDimension
{
    Financial = 0,
    Operational = 1,
    Reputational = 2,
    Legal = 3,
    Client = 4
}

public enum ControlType
{
    Preventive = 0,
    Detective = 1,
    Corrective = 2
}

public enum ControlFrequency
{
    Daily = 0,
    Weekly = 1,
    Monthly = 2,
    Quarterly = 3,
    Annually = 4
}
