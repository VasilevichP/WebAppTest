using WebAppTest.Enums;

namespace WebAppTest.DTO;

public class FilterBookingDTO
{
    public string? UserEmail { get; set; }
    public string? QuestTitle { get; set; }
    public DateOnly? DateMin { get; set; }
    public DateOnly? DateMax { get; set; }
    public BookingStatus? Status { get; set; }
}