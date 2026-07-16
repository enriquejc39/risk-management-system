using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Entities;

namespace RiskManagement.Infrastructure.Data;

public class RiskDbContext : DbContext
{
    public RiskDbContext(DbContextOptions<RiskDbContext> options) : base(options) { }

    public DbSet<Risk> Risks => Set<Risk>();
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<RiskCategory> RiskCategories => Set<RiskCategory>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Evidence> Evidences => Set<Evidence>();
    public DbSet<Control> Controls => Set<Control>();
    public DbSet<RiskControl> RiskControls => Set<RiskControl>();
    public DbSet<ActionPlan> ActionPlans => Set<ActionPlan>();
    public DbSet<Methodology> Methodologies => Set<Methodology>();
    public DbSet<Scale> Scales => Set<Scale>();
    public DbSet<Questionnaire> Questionnaires => Set<Questionnaire>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditStandard> AuditStandards => Set<AuditStandard>();
    public DbSet<AuditRequirement> AuditRequirements => Set<AuditRequirement>();
    public DbSet<RiskAuditRequirement> RiskAuditRequirements => Set<RiskAuditRequirement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Risk>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DynamicResponses).HasColumnType("jsonb");
            entity.HasIndex(e => e.AreaId);
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => e.RiskOwnerId);
            entity.HasIndex(e => e.RiskScore);
            entity.HasIndex(e => e.Status);
            entity.HasMany(e => e.Evidences)
                .WithOne(e => e.Risk)
                .HasForeignKey(e => e.RiskId);
            entity.HasMany(e => e.ActionPlans)
                .WithOne(e => e.Risk)
                .HasForeignKey(e => e.RiskId);
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.ParentArea)
                .WithMany()
                .HasForeignKey(e => e.ParentAreaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RiskCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Area)
                .WithMany(a => a.Categories)
                .HasForeignKey(e => e.AreaId);
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.EntityType, e.EntityId });
            entity.HasIndex(e => e.Timestamp);
        });

        modelBuilder.Entity<Evidence>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ContentType).HasMaxLength(200);
            entity.HasOne(e => e.Risk)
                .WithMany(r => r.Evidences)
                .HasForeignKey(e => e.RiskId);
        });

        modelBuilder.Entity<Control>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.ResponsibleArea);
        });

        modelBuilder.Entity<RiskControl>(entity =>
        {
            entity.HasKey(e => new { e.RiskId, e.ControlId });
            entity.HasOne(e => e.Risk)
                .WithMany(r => r.RiskControls)
                .HasForeignKey(e => e.RiskId);
            entity.HasOne(e => e.Control)
                .WithMany(c => c.RiskControls)
                .HasForeignKey(e => e.ControlId);
        });

        modelBuilder.Entity<ActionPlan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.RiskId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CommitmentDate);
        });

        modelBuilder.Entity<Methodology>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Scale>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Methodology)
                .WithMany(m => m.Scales)
                .HasForeignKey(e => e.MethodologyId);
        });

        modelBuilder.Entity<Questionnaire>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Area)
                .WithMany()
                .HasForeignKey(e => e.AreaId)
                .IsRequired(false);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Questionnaire)
                .WithMany(q => q.Questions)
                .HasForeignKey(e => e.QuestionnaireId);
            entity.HasIndex(e => e.Order);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.RecipientId);
            entity.HasIndex(e => e.IsSent);
            entity.HasIndex(e => e.ScheduledFor);
        });

        modelBuilder.Entity<AuditStandard>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<AuditRequirement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.AuditStandard)
                .WithMany(s => s.Requirements)
                .HasForeignKey(e => e.AuditStandardId);
            entity.HasIndex(e => e.RequirementCode);
        });

        modelBuilder.Entity<RiskAuditRequirement>(entity =>
        {
            entity.HasKey(e => new { e.RiskId, e.AuditRequirementId });
            entity.HasOne(e => e.Risk)
                .WithMany(r => r.AuditRequirements)
                .HasForeignKey(e => e.RiskId);
            entity.HasOne(e => e.AuditRequirement)
                .WithMany(r => r.RiskRequirements)
                .HasForeignKey(e => e.AuditRequirementId);
        });
    }
}
