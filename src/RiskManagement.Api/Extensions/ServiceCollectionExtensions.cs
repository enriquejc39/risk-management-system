using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Interfaces;
using RiskManagement.Infrastructure.Data;
using RiskManagement.Infrastructure.Repositories;
using RiskManagement.Infrastructure.Services;

namespace RiskManagement.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RiskDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IRiskRepository, RiskRepository>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IAiOrchestrator, AiOrchestrator>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", builder =>
                builder.WithOrigins(configuration.GetValue<string>("Frontend:Url") ?? "http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        return services;
    }

    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Entra:Authority"];
                options.Audience = configuration["Entra:Audience"];
            });
        services.AddAuthorization();

        return services;
    }
}
