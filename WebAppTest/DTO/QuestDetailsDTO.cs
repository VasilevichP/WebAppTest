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
    public List<ReviewDTO> Reviews { get; set; } = new();
    public double Rating { get; set; }
    
    public static QuestDetailsDTO ToDTO(Quest quest)
    {
        return new QuestDetailsDTO
        {
            Id = quest.Id,
            Title = quest.Title,
            Description = quest.Description,
            Address = quest.Address,
            DurationMinutes = quest.DurationMinutes,
            MaxParticipants = quest.MaxParticipants,
            Price = quest.Price,
            DifficultyLevel = quest.DifficultyLevel,
            HasActors = quest.HasActors,
            FearLevel = quest.FearLevel,
            PhotoUrls = quest.Photos.Select(p => p.Url).ToList(),
            Reviews = quest.Reviews.Select(ReviewDTO.ToDTO).ToList(),
            Rating = quest.Rating
        };
    }
}