namespace WebAppTest.DTO;

public class BookingQuestDTO
{
    public Guid Id { get; set; }
    public string UserN { get; set; } = null!;
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public bool IsCompleted { get; set; }
}