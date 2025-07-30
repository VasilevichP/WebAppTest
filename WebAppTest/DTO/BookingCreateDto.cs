using WebAppTest.Entities;

namespace WebAppTest.DTO;

public class BookingCreateDto
{
    public Guid QuestId { get; set; }
    public Guid? UserId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int ParticipantsCount { get; set; }
}