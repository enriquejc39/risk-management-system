using Microsoft.AspNetCore.Mvc;
using RiskManagement.Core.Entities;
using RiskManagement.Core.Interfaces;

namespace RiskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionnairesController : ControllerBase
{
    private readonly IQuestionnaireRepository _questionnaireRepo;
    private readonly IRiskRepository _riskRepo;
    private readonly IAuditService _auditService;

    public QuestionnairesController(
        IQuestionnaireRepository questionnaireRepo,
        IRiskRepository riskRepo,
        IAuditService auditService)
    {
        _questionnaireRepo = questionnaireRepo;
        _riskRepo = riskRepo;
        _auditService = auditService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _questionnaireRepo.GetAllAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var q = await _questionnaireRepo.GetFullAsync(id);
        return q is null ? NotFound() : Ok(q);
    }

    [HttpGet("area/{areaId:guid?}")]
    public async Task<IActionResult> GetByArea(Guid? areaId) =>
        Ok(await _questionnaireRepo.GetByAreaAsync(areaId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Questionnaire questionnaire)
    {
        questionnaire.Id = Guid.NewGuid();
        foreach (var q in questionnaire.Questions)
        {
            q.Id = Guid.NewGuid();
            q.QuestionnaireId = questionnaire.Id;
        }
        var created = await _questionnaireRepo.AddAsync(questionnaire);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitRequest request)
    {
        var questionnaire = await _questionnaireRepo.GetFullAsync(request.QuestionnaireId);
        if (questionnaire is null) return NotFound("Cuestionario no encontrado");

        var risk = new Risk
        {
            Id = Guid.NewGuid(),
            Name = request.Answers.TryGetValue("name", out var name) ? name : "Riesgo sin nombre",
            Description = request.Answers.TryGetValue("description", out var desc) ? desc : null,
            AreaId = request.AreaId,
            CategoryId = request.CategoryId,
            RiskOwnerId = request.RiskOwnerId,
            Probability = request.Answers.TryGetValue("probability", out var prob) && int.TryParse(prob, out var p) ? p : 1,
            Impact = request.Answers.TryGetValue("impact", out var imp) && int.TryParse(imp, out var i) ? i : 1,
            Status = RiskStatus.Submitted,
            DynamicResponses = System.Text.Json.JsonSerializer.SerializeToDocument(request.Answers),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = User.Identity?.Name ?? "system"
        };

        var created = await _riskRepo.AddAsync(risk);
        await _auditService.LogAsync("Risk", created.Id, "Created",
            User.Identity?.Name ?? "system", User.Identity?.Name,
            newValues: new { created.Name, created.Probability, created.Impact, Source = "Questionnaire" });

        return CreatedAtAction("GetById", "Risks", new { id = created.Id }, created);
    }

    [HttpPost("{id:guid}/questions")]
    public async Task<IActionResult> AddQuestion(Guid id, [FromBody] Question question)
    {
        var q = await _questionnaireRepo.GetByIdAsync(id);
        if (q is null) return NotFound();
        question.Id = Guid.NewGuid();
        question.QuestionnaireId = id;
        await _questionnaireRepo.UpdateAsync(q);
        return Ok(question);
    }
}

public record SubmitRequest(
    Guid QuestionnaireId,
    Guid AreaId,
    Guid CategoryId,
    Guid RiskOwnerId,
    Dictionary<string, string> Answers);
