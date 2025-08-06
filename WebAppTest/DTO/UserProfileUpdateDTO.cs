using Microsoft.AspNetCore.Identity;
using WebAppTest.Entities;

namespace WebAppTest.DTO;

public class UserProfileUpdateDTO
{
    public string? NewEmail { get; set; }
    public string? NewUsername { get; set; }
    public string? NewPhone { get; set; }
    public string? NewPassword { get; set; }
        
    public static void FromDTO(User user, UserProfileUpdateDTO dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.NewEmail))
            user.Email = dto.NewEmail;
        
        if (!string.IsNullOrWhiteSpace(dto.NewUsername))
            user.Username = dto.NewUsername;
        
        if (!string.IsNullOrWhiteSpace(dto.NewPhone))
            user.Phone = dto.NewPhone;
        
        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            user.PasswordHash = new PasswordHasher<User>()
                .HashPassword(user, dto.NewPassword);
    }
}