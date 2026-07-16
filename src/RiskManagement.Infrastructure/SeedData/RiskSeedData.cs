using Microsoft.EntityFrameworkCore;
using RiskManagement.Core.Entities;
using RiskManagement.Infrastructure.Data;

namespace RiskManagement.Infrastructure.SeedData;

public static class RiskSeedData
{
    public static async Task SeedAsync(RiskDbContext context)
    {
        if (await context.Areas.AnyAsync()) return;

        var areas = new List<Area>
        {
            new() { Id = Guid.NewGuid(), Name = "Operaciones", Description = "Operaciones del negocio" },
            new() { Id = Guid.NewGuid(), Name = "Tecnología", Description = "Área de TI y sistemas" },
            new() { Id = Guid.NewGuid(), Name = "Finanzas", Description = "Gestión financiera" },
            new() { Id = Guid.NewGuid(), Name = "Recursos Humanos", Description = "Gestión de personas" },
            new() { Id = Guid.NewGuid(), Name = "Legal", Description = "Cumplimiento legal" },
            new() { Id = Guid.NewGuid(), Name = "Calidad", Description = "Gestión de calidad" },
            new() { Id = Guid.NewGuid(), Name = "Seguridad de la Información", Description = "Ciberseguridad y protección de datos" },
            new() { Id = Guid.NewGuid(), Name = "Compliance", Description = "Cumplimiento normativo" },
        };

        await context.Areas.AddRangeAsync(areas);
        await context.SaveChangesAsync();

        var categories = new List<RiskCategory>
        {
            new() { Id = Guid.NewGuid(), Name = "Continuidad Operacional", AreaId = areas[0].Id },
            new() { Id = Guid.NewGuid(), Name = "Caída de Sistemas", AreaId = areas[0].Id },
            new() { Id = Guid.NewGuid(), Name = "Ausentismo", AreaId = areas[0].Id },
            new() { Id = Guid.NewGuid(), Name = "Ciberseguridad", AreaId = areas[1].Id },
            new() { Id = Guid.NewGuid(), Name = "Infraestructura", AreaId = areas[1].Id },
            new() { Id = Guid.NewGuid(), Name = "Proveedores TI", AreaId = areas[1].Id },
            new() { Id = Guid.NewGuid(), Name = "Liquidez", AreaId = areas[2].Id },
            new() { Id = Guid.NewGuid(), Name = "Fraude", AreaId = areas[2].Id },
            new() { Id = Guid.NewGuid(), Name = "Rotación", AreaId = areas[3].Id },
            new() { Id = Guid.NewGuid(), Name = "Clima Laboral", AreaId = areas[3].Id },
            new() { Id = Guid.NewGuid(), Name = "Cumplimiento Regulatorio", AreaId = areas[4].Id },
            new() { Id = Guid.NewGuid(), Name = "Contratos", AreaId = areas[4].Id },
            new() { Id = Guid.NewGuid(), Name = "SLA", AreaId = areas[5].Id },
            new() { Id = Guid.NewGuid(), Name = "No Conformidades", AreaId = areas[5].Id },
            new() { Id = Guid.NewGuid(), Name = "Fuga de Información", AreaId = areas[6].Id },
            new() { Id = Guid.NewGuid(), Name = "Incumplimiento Regulatorio", AreaId = areas[7].Id },
        };

        await context.RiskCategories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        var controls = new List<Control>
        {
            new() { Id = Guid.NewGuid(), Name = "Backup Diario", Description = "Backup automatizado diario de datos críticos", ControlType = "Preventivo", Frequency = "Diario", ResponsibleArea = "Tecnología", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
            new() { Id = Guid.NewGuid(), Name = "Monitoreo 24/7", Description = "Monitoreo continuo de infraestructura", ControlType = "Detectivo", Frequency = "Diario", ResponsibleArea = "Tecnología", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
            new() { Id = Guid.NewGuid(), Name = "Autenticación MFA", Description = "Autenticación multifactor para accesos críticos", ControlType = "Preventivo", Frequency = "Diario", ResponsibleArea = "Seguridad de la Información", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
            new() { Id = Guid.NewGuid(), Name = "Auditoría de Accesos", Description = "Revisión trimestral de accesos a sistemas", ControlType = "Detectivo", Frequency = "Trimestral", ResponsibleArea = "Seguridad de la Información", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
            new() { Id = Guid.NewGuid(), Name = "Plan de Continuidad", Description = "Plan de continuidad operacional actualizado", ControlType = "Correctivo", Frequency = "Mensual", ResponsibleArea = "Operaciones", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
            new() { Id = Guid.NewGuid(), Name = "Capacitación Anual", Description = "Capacitación anual en seguridad y compliance", ControlType = "Preventivo", Frequency = "Anual", ResponsibleArea = "Recursos Humanos", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
            new() { Id = Guid.NewGuid(), Name = "DRP - Disaster Recovery", Description = "Plan de recuperación ante desastres", ControlType = "Correctivo", Frequency = "Mensual", ResponsibleArea = "Tecnología", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
            new() { Id = Guid.NewGuid(), Name = "Segregación de Funciones", Description = "Controles de segregación en procesos financieros", ControlType = "Preventivo", Frequency = "Diario", ResponsibleArea = "Finanzas", IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
        };

        await context.Controls.AddRangeAsync(controls);
        await context.SaveChangesAsync();

        var methodologies = new List<Methodology>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Metodología Estándar",
                Description = "Metodología por defecto de 5 niveles",
                IsActive = true,
                Scales = new List<Scale>
                {
                    new() { Id = Guid.NewGuid(), Dimension = "Probability", Name = "Muy Baja", Value = 1, Description = "Improbable - < 10%" },
                    new() { Id = Guid.NewGuid(), Dimension = "Probability", Name = "Baja", Value = 2, Description = "Poco probable - 10-30%" },
                    new() { Id = Guid.NewGuid(), Dimension = "Probability", Name = "Media", Value = 3, Description = "Posible - 30-50%" },
                    new() { Id = Guid.NewGuid(), Dimension = "Probability", Name = "Alta", Value = 4, Description = "Probable - 50-70%" },
                    new() { Id = Guid.NewGuid(), Dimension = "Probability", Name = "Muy Alta", Value = 5, Description = "Casi segura - >70%" },
                    new() { Id = Guid.NewGuid(), Dimension = "Impact", Name = "Muy Bajo", Value = 1, Description = "Impacto mínimo" },
                    new() { Id = Guid.NewGuid(), Dimension = "Impact", Name = "Bajo", Value = 2, Description = "Impacto menor" },
                    new() { Id = Guid.NewGuid(), Dimension = "Impact", Name = "Medio", Value = 3, Description = "Impacto moderado" },
                    new() { Id = Guid.NewGuid(), Dimension = "Impact", Name = "Alto", Value = 4, Description = "Impacto mayor" },
                    new() { Id = Guid.NewGuid(), Dimension = "Impact", Name = "Muy Alto", Value = 5, Description = "Impacto crítico" },
                }
            }
        };

        await context.Methodologies.AddRangeAsync(methodologies);
        await context.SaveChangesAsync();

        var auditStandards = new List<AuditStandard>
        {
            new()
            {
                Id = Guid.NewGuid(), Code = "ISO9001", Name = "ISO 9001:2015 - Gestión de Calidad",
                Description = "Sistemas de gestión de calidad - Requisitos",
                Requirements = new List<AuditRequirement>
                {
                    new() { Id = Guid.NewGuid(), RequirementCode = "4.1", Description = "Comprensión de la organización y su contexto", Category = "Contexto" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "5.1", Description = "Liderazgo y compromiso", Category = "Liderazgo" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "6.1", Description = "Acciones para abordar riesgos y oportunidades", Category = "Planificación" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "7.1", Description = "Recursos", Category = "Soporte" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "8.1", Description = "Planificación y control operacional", Category = "Operación" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "9.1", Description = "Seguimiento, medición, análisis y evaluación", Category = "Evaluación" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "10.1", Description = "No conformidad y acción correctiva", Category = "Mejora" },
                }
            },
            new()
            {
                Id = Guid.NewGuid(), Code = "ISO27001", Name = "ISO 27001:2022 - Seguridad de la Información",
                Description = "Sistemas de gestión de seguridad de la información",
                Requirements = new List<AuditRequirement>
                {
                    new() { Id = Guid.NewGuid(), RequirementCode = "A.5.1", Description = "Políticas de seguridad de la información", Category = "Políticas" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "A.6.1", Description = "Organización interna", Category = "Organización" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "A.8.1", Description = "Gestión de activos", Category = "Activos" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "A.9.1", Description = "Control de acceso", Category = "Acceso" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "A.12.1", Description = "Gestión de incidentes", Category = "Incidentes" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "A.16.1", Description = "Gestión de incidentes de seguridad", Category = "Incidentes" },
                }
            },
            new()
            {
                Id = Guid.NewGuid(), Code = "ISO22301", Name = "ISO 22301 - Continuidad del Negocio",
                Description = "Sistemas de gestión de continuidad del negocio",
                Requirements = new List<AuditRequirement>
                {
                    new() { Id = Guid.NewGuid(), RequirementCode = "4.1", Description = "Contexto de la organización", Category = "Contexto" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "8.1", Description = "Planificación de continuidad", Category = "Planificación" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "8.2", Description = "Procedimientos de respuesta", Category = "Respuesta" },
                }
            },
            new()
            {
                Id = Guid.NewGuid(), Code = "ISO31000", Name = "ISO 31000 - Gestión del Riesgo",
                Description = "Principios y directrices para la gestión del riesgo",
                Requirements = new List<AuditRequirement>
                {
                    new() { Id = Guid.NewGuid(), RequirementCode = "5.1", Description = "Establecimiento del contexto", Category = "Contexto" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "5.2", Description = "Identificación del riesgo", Category = "Identificación" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "5.3", Description = "Análisis del riesgo", Category = "Análisis" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "5.4", Description = "Evaluación del riesgo", Category = "Evaluación" },
                    new() { Id = Guid.NewGuid(), RequirementCode = "5.5", Description = "Tratamiento del riesgo", Category = "Tratamiento" },
                }
            },
        };

        await context.AuditStandards.AddRangeAsync(auditStandards);
        await context.SaveChangesAsync();

        var operacionesArea = areas[0];
        var tecnologiaArea = areas[1];

        var questionnaires = new List<Questionnaire>
        {
            new()
            {
                Id = Guid.NewGuid(), Name = "Evaluación de Riesgos - Operaciones",
                Description = "Cuestionario para identificar riesgos en operaciones",
                AreaId = operacionesArea.Id, IsActive = true,
                Questions = new List<Question>
                {
                    new() { Id = Guid.NewGuid(), Text = "¿Qué podría afectar el cumplimiento de SLA?", QuestionType = "select", Options = "Caída de sistema|Ausentismo|Error humano|Falta de capacitación|Problemas de proveedor", Order = 1, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "¿Cuál es la probabilidad de que ocurra?", QuestionType = "select", Options = "1|2|3|4|5", Order = 2, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "¿Cuál sería el impacto operacional?", QuestionType = "select", Options = "1|2|3|4|5", Order = 3, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "¿Qué controles existen actualmente?", QuestionType = "textarea", Options = "", Order = 4, IsRequired = false },
                    new() { Id = Guid.NewGuid(), Text = "Nombre del riesgo", QuestionType = "text", Options = "", Order = 0, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "Descripción detallada", QuestionType = "textarea", Options = "", Order = 5, IsRequired = false },
                }
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "Evaluación de Riesgos - Tecnología",
                Description = "Cuestionario para identificar riesgos de TI",
                AreaId = tecnologiaArea.Id, IsActive = true,
                Questions = new List<Question>
                {
                    new() { Id = Guid.NewGuid(), Text = "¿Qué podría afectar la disponibilidad del servicio?", QuestionType = "select", Options = "Falla de servidores|Problemas de red|Ciberataques|Falla de proveedor|Error de configuración", Order = 1, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "¿Cuál es la probabilidad de ocurrencia?", QuestionType = "select", Options = "1|2|3|4|5", Order = 2, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "¿Cuál sería el impacto?", QuestionType = "select", Options = "1|2|3|4|5", Order = 3, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "¿Qué controles de seguridad existen?", QuestionType = "textarea", Options = "", Order = 4, IsRequired = false },
                    new() { Id = Guid.NewGuid(), Text = "Nombre del riesgo", QuestionType = "text", Options = "", Order = 0, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "Descripción del riesgo", QuestionType = "textarea", Options = "", Order = 5, IsRequired = false },
                }
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "Evaluación de Riesgos - General",
                Description = "Cuestionario genérico para cualquier área",
                AreaId = null, IsActive = true,
                Questions = new List<Question>
                {
                    new() { Id = Guid.NewGuid(), Text = "Nombre del riesgo", QuestionType = "text", Options = "", Order = 0, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "¿Qué podría salir mal en tu proceso?", QuestionType = "textarea", Options = "", Order = 1, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "Probabilidad (1=Muy Baja, 5=Muy Alta)", QuestionType = "select", Options = "1|2|3|4|5", Order = 2, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "Impacto (1=Muy Bajo, 5=Muy Alto)", QuestionType = "select", Options = "1|2|3|4|5", Order = 3, IsRequired = true },
                    new() { Id = Guid.NewGuid(), Text = "Controles existentes", QuestionType = "textarea", Options = "", Order = 4, IsRequired = false },
                    new() { Id = Guid.NewGuid(), Text = "Descripción y contexto", QuestionType = "textarea", Options = "", Order = 5, IsRequired = false },
                }
            },
        };

        await context.Questionnaires.AddRangeAsync(questionnaires);
        await context.SaveChangesAsync();
    }
}
