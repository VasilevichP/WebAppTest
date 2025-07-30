using WebAppTest.Entities;
using WebAppTest.Enums;

namespace WebAppTest.DTO;

public class BookingDTO
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; } = null!;
    public string QuestTitle { get; set; } = null!;
    public DateOnly Date { get; set; }
    public TimeOnly Time { get; set; }
    public int ParticipantsCount { get; set; }
    public BookingStatus Status { get; set; }
    
    public static BookingDTO ToDTO(Booking booking)
    {
        return new BookingDTO
        {
            Id = booking.Id,
            UserEmail = booking.User.Email,
            QuestTitle = booking.Quest.Title,
            ParticipantsCount = booking.ParticipantsCount,
            Date = booking.Date,
            Time = booking.Time,
            Status = booking.Status
        };
    }
}