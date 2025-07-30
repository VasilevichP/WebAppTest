using WebAppTest.Entities;

namespace WebAppTest.DTO;

public class UserProfileDTO
{
    public string Email { get; set; } = String.Empty;
    public string PasswordMasked => new string('*', 10); // маска
    
    public string? Username { get; set; }
    public string Phone { get; set; } = String.Empty;
    
    public List<BookingDTO> Bookings { get; set; } = new List<BookingDTO>();
    public List<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();
}