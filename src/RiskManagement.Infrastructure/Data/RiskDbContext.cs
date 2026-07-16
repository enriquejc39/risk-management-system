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
            entity.HasOne(e => e.Risk)
                .WithMany()
                .HasForeignKey(e => e.RiskId);
        });
    }
}
