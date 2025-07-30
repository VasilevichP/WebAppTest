namespace WebAppTest.DTO;

public class UserBriefDTO
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = String.Empty;
    
    public string? Username { get; set; }
    public string Phone { get; set; } = String.Empty;
}