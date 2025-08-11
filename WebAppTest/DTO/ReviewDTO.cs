using WebAppTest.Entities;

namespace WebAppTest.DTO;

public class ReviewDTO
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string QuestTitle { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid QuestId { get; set; }
    public int Rating { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}