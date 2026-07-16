using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Infrastructure.Services;

public class KpiService : IKpiService
{
    private readonly RiskDbContext _context;

    public KpiService(RiskDbContext context) => _context = context;

    public async Task<KpiDashboard> GetDashboardAsync()
    {
        var risks = await _context.Risks.AsNoTracking().ToListAsync();
        var actionPlans = await _context.ActionPlans.AsNoTracking().ToListAsync();
        var areas = await _context.Areas.AsNoTracking().ToListAsync();

        var now = DateTime.UtcNow;
        var ninetyDaysAgo = now.AddDays(-90);

        var dashboard = new KpiDashboard
        {
            TotalRisks = risks.Count,
            CriticalRisks = risks.Count(r => r.Level == RiskLevel.Critical),
            HighRisks = risks.Count(r => r.Level == RiskLevel.High),
            OverdueRisks = risks.Count(r =>
                r.LastReviewDate == null || r.LastReviewDate < ninetyDaysAgo),
            RisksWithoutEvidence = risks.Count(r =>
                !_context.Evidences.Any(e => e.RiskId == r.Id)),
            RisksWithoutReview = risks.Count(r =>
                r.LastReviewDate == null || r.LastReviewDate < ninetyDaysAgo),
            OverdueActionPlans = actionPlans.Count(a =>
                a.CommitmentDate < now
                && a.Status != ActionPlanStatus.Completed
                && a.Status != ActionPlanStatus.Cancelled),
            CompletedActionPlans = actionPlans.Count(a => a.Status == ActionPlanStatus.Completed),
            TotalActionPlans = actionPlans.Count,
            AverageResidualRisk = risks.Count > 0 ? risks.Average(r => r.RiskScore) : 0,
            ComplianceRate = actionPlans.Count > 0
                ? (double)actionPlans.Count(a => a.Status == ActionPlanStatus.Completed) / actionPlans.Count * 100
                : 0,
            TopRisks = risks
                .OrderByDescending(r => r.RiskScore)
                .Take(10)
                .Select(r => new RiskTrend
                {
                    RiskName = r.Name,
                    Score = r.RiskScore,
                    Level = r.Level.ToString(),
                    Area = areas.FirstOrDefault(a => a.Id == r.AreaId)?.Name ?? ""
                })
                .ToList(),
            MadurezPorArea = areas.Select(a =>
            {
                var areaRisks = risks.Where(r => r.AreaId == a.Id).ToList();
                var areaPlans = actionPlans.Where(p =>
                    areaRisks.Any(r => r.Id == p.RiskId)).ToList();
                return new AreaMadurez
                {
                    AreaName = a.Name,
                    TotalRisks = areaRisks.Count,
                    CriticalRisks = areaRisks.Count(r => r.Level == RiskLevel.Critical),
                    AverageScore = areaRisks.Count > 0 ? areaRisks.Average(r => r.RiskScore) : 0,
                    ComplianceRate = areaPlans.Count > 0
                        ? (double)areaPlans.Count(p => p.Status == ActionPlanStatus.Completed) / areaPlans.Count * 100
                        : 100,
                    Nivel = GetMadurezNivel(areaRisks)
                };
            }).ToList()
        };

        return dashboard;
    }

    public async Task<IReadOnlyList<RiskTrend>> GetTopRisksAsync(int count = 10)
    {
        var risks = await _context.Risks
            .OrderByDescending(r => r.RiskScore)
            .Take(count)
            .AsNoTracking()
            .ToListAsync();

        var areas = await _context.Areas.AsNoTracking().ToListAsync();

        return risks.Select(r => new RiskTrend
        {
            RiskName = r.Name,
            Score = r.RiskScore,
            Level = r.Level.ToString(),
            Area = areas.FirstOrDefault(a => a.Id == r.AreaId)?.Name ?? ""
        }).ToList();
    }

    private static string GetMadurezNivel(List<Risk> risks)
    {
        if (risks.Count == 0) return "Sin datos";
        var avg = risks.Average(r => r.RiskScore);
        return avg switch
        {
            >= 15 => "Crítico",
            >= 10 => "En desarrollo",
            >= 5 => "Aceptable",
            _ => "Maduro"
        };
    }
}
