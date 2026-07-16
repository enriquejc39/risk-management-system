namespace RiskManagement.Core.Entities;

public class Evidence
{
    public Guid Id { get; set; }
    public Guid RiskId { get; set; }
    public Risk Risk { get; set; } = null!;
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public int Version { get; set; }
    public DateTime UploadedAt { get; set; }
    public string UploadedBy { get; set; } = string.Empty;
}
