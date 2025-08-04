using WebAppTest.Entities;

namespace WebAppTest.DTO;

public class QuestUpdateCreateDTO
{
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string Address { get; set; } = String.Empty;
    public int DurationMinutes { get; set; }
    public int MaxParticipants { get; set; }
    public decimal Price { get; set; }
    public int DifficultyLevel { get; set; }
    public bool HasActors { get; set; } 
    public int FearLevel { get; set; }

    public List<string> PhotoUrls { get; set; } = new(6);
}