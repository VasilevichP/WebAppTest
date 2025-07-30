namespace WebAppTest.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = String.Empty;
    public string PasswordHash { get; set; } = String.Empty;
    
    public string? Username { get; set; }
    
    public string Role { get; set; }
    
    public string Phone { get; set; } = String.Empty;
    
    public ICollection<Booking> Bookings { get; set; }
    public ICollection<Review> Reviews { get; set; }
    
    public string? RefreshToken { get; set; } 
    
    public DateTime RefreshTokenExpiryTime { get; set; }
}