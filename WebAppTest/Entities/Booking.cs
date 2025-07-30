using WebAppTest.Enums;

namespace WebAppTest.Entities;

public class Booking
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid QuestId { get; set; }
    public Quest Quest { get; set; } = null!;

    
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int ParticipantsCount { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.ACTIVE;
}