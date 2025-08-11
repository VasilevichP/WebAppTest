namespace WebAppTest.DTO;

public class UserBaseDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string Phone { get; set; } = string.Empty;
}