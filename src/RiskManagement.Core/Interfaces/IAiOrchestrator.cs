namespace RiskManagement.Core.Interfaces;

public interface IAiOrchestrator
{
    Task<string> GetRiskSuggestionAsync(string question, Dictionary<string, string>? context = null);
    Task<string> AutocompleteFormAsync(string field, string currentValue, Dictionary<string, string> riskContext);
}
