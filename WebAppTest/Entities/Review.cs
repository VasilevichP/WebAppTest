namespace WebAppTest.Entities;

public class Review
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid QuestId { get; set; }
    public Quest Quest { get; set; } = null!;

    public int Rating { get; set; }
    public string Text { get; set; } = String.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
}