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
    
    public static Quest FromCreateDTO(QuestUpdateCreateDTO dto)
    {
        return new Quest
        {
            Title = dto.Title,
            Description = dto.Description,
            Address = dto.Address,
            DurationMinutes = dto.DurationMinutes,
            MaxParticipants = dto.MaxParticipants,
            Price = dto.Price,
            DifficultyLevel = dto.DifficultyLevel,
            HasActors = dto.HasActors,
            FearLevel = dto.FearLevel,
            Photos = dto.PhotoUrls.Select(url => new QuestPhoto { Url = url }).ToList()
        };
    }
    public static void FromUpdateDTO(Quest quest, QuestUpdateCreateDTO dto)
    {
        quest.Title = dto.Title;
        quest.Description = dto.Description;
        quest.Address = dto.Address;
        quest.DurationMinutes = dto.DurationMinutes;
        quest.MaxParticipants = dto.MaxParticipants;
        quest.Price = dto.Price;
        quest.DifficultyLevel = dto.DifficultyLevel;
        quest.HasActors = dto.HasActors;
        quest.FearLevel = dto.FearLevel;

        quest.Photos = dto.PhotoUrls
            .Select(url => new QuestPhoto
            {
                Url = url,
                Quest = quest
            })
            .ToList();
    }
}