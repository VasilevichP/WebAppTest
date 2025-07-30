using WebAppTest.Entities;

namespace WebAppTest.DTO;

public class UserAdminDTO
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = String.Empty;
    
    public string? Username { get; set; }
    public string Phone { get; set; } = String.Empty;
    
    public List<BookingDTO> Bookings { get; set; } = new List<BookingDTO>();
    public List<Review> Reviews { get; set; } = new List<Review>();
}