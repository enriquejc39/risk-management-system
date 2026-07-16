using Microsoft.AspNetCore.Mvc;
using RiskManagement.Core.Interfaces;

namespace RiskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AiController : ControllerBase
{
    private readonly IAiOrchestrator _aiOrchestrator;

    public AiController(IAiOrchestrator aiOrchestrator) => _aiOrchestrator = aiOrchestrator;

    [HttpPost("suggest")]
    public async Task<IActionResult> Suggest([FromBody] SuggestRequest request)
    {
        var suggestion = await _aiOrchestrator.GetRiskSuggestionAsync(request.Question, request.Context);
        return Ok(new { suggestion });
    }

    [HttpPost("autocomplete")]
    public async Task<IActionResult> Autocomplete([FromBody] AutocompleteRequest request)
    {
        var suggestion = await _aiOrchestrator.AutocompleteFormAsync(
            request.Field, request.CurrentValue, request.Context);
        return Ok(new { suggestion });
    }
}

public record SuggestRequest(string Question, Dictionary<string, string>? Context);
public record AutocompleteRequest(string Field, string CurrentValue, Dictionary<string, string> Context);
