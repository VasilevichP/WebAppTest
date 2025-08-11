using System.ComponentModel.DataAnnotations;

namespace WebAppTest.DTO;

public class UserAuthDTO
{
    [EmailAddress (ErrorMessage = "Неправильный формат адреса эл. почты")]
    public string Email { get; set; } = String.Empty;
    
    // [MinLength(6) (ErrorMessage = "Пароль должен содержать минимум 6 символов")]
    public string Password { get; set; } = String.Empty;
}