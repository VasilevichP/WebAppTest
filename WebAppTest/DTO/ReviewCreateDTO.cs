namespace WebAppTest.DTO;

public class ReviewCreateDTO
{
    public Guid? UserId { get; set; }
    public Guid QuestId { get; set; }
    public int Rating { get; set; }
    public string? Text { get; set; }
}