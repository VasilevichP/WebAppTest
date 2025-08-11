namespace WebAppTest.DTO;

public class UserProfileDTO : UserBaseDTO
{
    public List<BookingDTO> Bookings { get; set; } = new();
    public List<ReviewDTO> Reviews { get; set; } = new();
}