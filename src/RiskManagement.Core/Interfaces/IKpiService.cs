namespace RiskManagement.Core.Interfaces;

public class KpiDashboard
{
    public int TotalRisks { get; set; }
    public int CriticalRisks { get; set; }
    public int HighRisks { get; set; }
    public int OverdueRisks { get; set; }
    public int RisksWithoutEvidence { get; set; }
    public int RisksWithoutReview { get; set; }
    public double ComplianceRate { get; set; }
    public double AverageResidualRisk { get; set; }
    public int OverdueActionPlans { get; set; }
    public int CompletedActionPlans { get; set; }
    public int TotalActionPlans { get; set; }
    public List<AreaMadurez> MadurezPorArea { get; set; } = new();
    public List<RiskTrend> TopRisks { get; set; } = new();
}

public class AreaMadurez
{
    public string AreaName { get; set; } = string.Empty;
    public int TotalRisks { get; set; }
    public int CriticalRisks { get; set; }
    public double AverageScore { get; set; }
    public double ComplianceRate { get; set; }
    public string Nivel { get; set; } = string.Empty;
}

public class RiskTrend
{
    public string RiskName { get; set; } = string.Empty;
    public int Score { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
}

public interface IKpiService
{
    Task<KpiDashboard> GetDashboardAsync();
    Task<IReadOnlyList<RiskTrend>> GetTopRisksAsync(int count = 10);
}
