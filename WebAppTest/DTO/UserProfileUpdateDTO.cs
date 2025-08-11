using System.ComponentModel.DataAnnotations;

namespace WebAppTest.DTO;

public class UserProfileUpdateDTO
{
    [EmailAddress]
    public string? NewEmail { get; set; }
    
    [StringLength(50)]
    public string? NewUsername { get; set; }
    
    [RegularExpression("^[0-9]{12}$")]
    public string? NewPhone { get; set; }
    
    [MinLength(6)]
    public string? NewPassword { get; set; }
}