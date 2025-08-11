using WebAppTest.DTO;

namespace WebAppTest.Services;

public interface IUserService
{
    public Task<List<UserBaseDTO>> GetAllUsers();
    public Task<List<UserBaseDTO>> GetAllUsersFiltered(FilterUsersDTO filter);
    public Task<UserProfileDTO> GetProfileAsync(Guid userId);
    public Task<UserProfileDTO> UpdateProfileAsync(Guid userId, UserProfileUpdateDTO dto);
    public Task DeleteProfileAsync(Guid userId);
}