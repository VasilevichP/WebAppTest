using WebAppTest.Enums;

namespace WebAppTest.DTO;

public class BookingUpdateDTO
{
    public DateOnly? Date { get; set; }
    public TimeOnly? Time { get; set; }
    public int? Participants { get; set; }
    public BookingStatus? Status { get; set; }
}