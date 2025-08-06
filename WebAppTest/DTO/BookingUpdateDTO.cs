using WebAppTest.Entities;
using WebAppTest.Enums;

namespace WebAppTest.DTO;

public class BookingUpdateDTO
{
    public DateOnly? Date { get; set; }
    public TimeOnly? Time { get; set; }
    public int? Participants { get; set; }
    public BookingStatus? Status { get; set; }
    
    public static void FromUpdateDTO(Booking booking, BookingUpdateDTO dto)
    {
        if (dto.Date.HasValue) booking.Date = dto.Date.Value;
        if (dto.Time.HasValue) booking.Time = dto.Time.Value;
        if (dto.Participants.HasValue) booking.ParticipantsCount = dto.Participants.Value;
        if (dto.Status.HasValue) booking.Status = dto.Status.Value;
    }
}