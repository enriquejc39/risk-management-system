namespace RiskManagement.Core.Entities;

public class Questionnaire
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? AreaId { get; set; }
    public Area? Area { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}

public class Question
{
    public Guid Id { get; set; }
    public Guid QuestionnaireId { get; set; }
    public Questionnaire Questionnaire { get; set; } = null!;
    public string Text { get; set; } = string.Empty;
    public string QuestionType { get; set; } = "select";
    public string? Options { get; set; }
    public int Order { get; set; }
    public string? RiskMapping { get; set; }
    public bool IsRequired { get; set; } = true;
}
