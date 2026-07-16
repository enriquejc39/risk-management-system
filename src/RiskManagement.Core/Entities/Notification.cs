namespace RiskManagement.Core.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string RecipientId { get; set; } = string.Empty;
    public string? RecipientEmail { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public Guid? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
    public bool IsSent { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ScheduledFor { get; set; }
}
