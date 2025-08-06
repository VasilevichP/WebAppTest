using WebAppTest.Entities;

namespace WebAppTest.DTO;

public class UserBriefDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = String.Empty;
    
    public string? Username { get; set; }
    public string Phone { get; set; } = String.Empty;
    
        
    public static UserBriefDTO ToDTO(User user)
    {
        return new UserBriefDTO()
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            Phone = user.Phone
        };
    }
}