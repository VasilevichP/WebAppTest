using WebAppTest.Entities;
using WebAppTest.Enums;

namespace WebAppTest.DTO;

public class BookingDTO
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int ParticipantsCount { get; set; }
    public BookingStatus Status { get; set; }
    public string UserEmail { get; set; } = null!;
    public string QuestTitle { get; set; } = null!;
}