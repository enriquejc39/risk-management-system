using RiskManagement.Core.Entities;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Infrastructure.SeedData;

public static class RiskSeedData
{
    public static async Task SeedAsync(RiskDbContext context)
    {
        if (context.Areas.Any()) return;

        var areas = new List<Area>
        {
            new() { Id = Guid.NewGuid(), Name = "Tecnología", Description = "Área de TI" },
            new() { Id = Guid.NewGuid(), Name = "Finanzas", Description = "Área financiera" },
            new() { Id = Guid.NewGuid(), Name = "Operaciones", Description = "Operaciones del negocio" },
            new() { Id = Guid.NewGuid(), Name = "Recursos Humanos", Description = "Gestión de personas" },
            new() { Id = Guid.NewGuid(), Name = "Legal", Description = "Cumplimiento legal" },
        };

        await context.Areas.AddRangeAsync(areas);
        await context.SaveChangesAsync();

        var categories = new List<RiskCategory>
        {
            new() { Id = Guid.NewGuid(), Name = "Ciberseguridad", AreaId = areas[0].Id },
            new() { Id = Guid.NewGuid(), Name = "Infraestructura", AreaId = areas[0].Id },
            new() { Id = Guid.NewGuid(), Name = "Liquidez", AreaId = areas[1].Id },
            new() { Id = Guid.NewGuid(), Name = "Cumplimiento", AreaId = areas[4].Id },
        };

        await context.RiskCategories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }
}
