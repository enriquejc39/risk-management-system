using System.Net.Http.Json;
using System.Text.Json;
using RiskManagement.Core.Interfaces;

namespace RiskManagement.Infrastructure.Services;

public class AiOrchestrator : IAiOrchestrator
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AiOrchestrator(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GetRiskSuggestionAsync(string question,
        Dictionary<string, string>? context = null)
    {
        var systemPrompt = @"Eres Risk Copilot, un asistente especializado en gestión de riesgos corporativos.
        Ayuda al usuario a evaluar riesgos proporcionando sugerencias basadas en la información del contexto.
        Responde de forma concisa y profesional. Enfócate en la probabilidad e impacto del riesgo.";

        var messages = new List<object>
        {
            new { role = "system", content = systemPrompt }
        };

        if (context is not null)
        {
            var contextStr = string.Join("\n", context.Select(kv => $"{kv.Key}: {kv.Value}"));
            messages.Add(new { role = "user", content = $"Contexto del riesgo:\n{contextStr}\n\nPregunta: {question}" });
        }
        else
        {
            messages.Add(new { role = "user", content = question });
        }

        var response = await _httpClient.PostAsJsonAsync(
            _configuration["Ai:Endpoint"] ?? "https://api.openai.com/v1/chat/completions",
            new
            {
                model = _configuration["Ai:Model"] ?? "gpt-4",
                messages,
                max_tokens = 500
            });

        if (!response.IsSuccessStatusCode)
            return "No pude generar una sugerencia en este momento. Por favor, intente de nuevo.";

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        return result.GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "Sin respuesta";
    }

    public async Task<string> AutocompleteFormAsync(string field, string currentValue,
        Dictionary<string, string> riskContext)
    {
        var prompt = $"Basado en la información del riesgo, sugiere un valor para el campo '{field}'. " +
                     $"Valor actual: '{currentValue}'. " +
                     $"Contexto: {string.Join(", ", riskContext.Select(kv => $"{kv.Key}={kv.Value}"))}";

        return await GetRiskSuggestionAsync(prompt);
    }
}
