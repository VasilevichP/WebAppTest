namespace WebAppTest.Entities;

public class Quest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string Address { get; set; } = String.Empty;
    public int DurationMinutes { get; set; }
    public int MaxParticipants { get; set; }
    public decimal Price { get; set; }

    public int DifficultyLevel { get; set; }
    public bool HasActors { get; set; }
    public int FearLevel { get; set; }
    public double Rating { get; set; }
    public ICollection<QuestPhoto> Photos { get; set; } = new List<QuestPhoto>(6);
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}