using WebAppTest.Entities;

namespace WebAppTest.DTO;

public class QuestDetailsDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public int MaxParticipants { get; set; }
    public decimal Price { get; set; }
    public int DifficultyLevel { get; set; }
    public bool HasActors { get; set; }
    public int FearLevel { get; set; }

    public List<string> PhotoUrls { get; set; } = new();
    public double Rating { get; set; }
}