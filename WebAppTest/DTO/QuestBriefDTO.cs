namespace WebAppTest.DTO;

public class QuestBriefDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public int MaxParticipants { get; set; }
    public decimal Price { get; set; }
    public int DifficultyLevel { get; set; }
    public bool HasActors { get; set; }
    public int FearLevel { get; set; }
}