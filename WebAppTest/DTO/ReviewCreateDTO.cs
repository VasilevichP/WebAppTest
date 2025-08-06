using WebAppTest.Entities;

namespace WebAppTest.DTO;

public class ReviewCreateDTO
{
    public Guid? UserId { get; set; }
    public Guid QuestId { get; set; }
    public int Rating { get; set; }
    public string? Text { get; set; }
    
    public static Review FromCreateDTO(ReviewCreateDTO dto)
    {
        return new Review()
        {
            UserId = dto.UserId.Value,
            QuestId = dto.QuestId,
            Text = dto.Text,
            Date = DateTime.Now,
            Rating = dto.Rating
        };
    }
}