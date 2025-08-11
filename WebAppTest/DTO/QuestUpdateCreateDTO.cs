using System.ComponentModel.DataAnnotations;

namespace WebAppTest.DTO;

public class QuestUpdateCreateDTO
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int DurationMinutes { get; set; }

    [Range(1, int.MaxValue)]
    public int MaxParticipants { get; set; }

    [Range(0, 100000)]
    public decimal Price { get; set; }

    [Range(0, 5)]
    public int DifficultyLevel { get; set; }

    public bool HasActors { get; set; }

    [Range(0, 5)]
    public int FearLevel { get; set; }
    public List<string> PhotoUrls { get; set; } = new(6);
}