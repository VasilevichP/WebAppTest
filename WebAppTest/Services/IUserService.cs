using WebAppTest.DTO;

namespace WebAppTest.Services;

public interface IUserService
{
    public Task<List<UserBriefDTO>> GetAllUsers();
    public Task<List<UserBriefDTO>> GetAllUsersFiltered(FilterUsersDTO filter);
    public Task<UserProfileDTO?> GetProfileAsync(Guid userId);
    public Task<UserAdminDTO?> GetProfileForAdminAsync(Guid userId);
    public Task<UserProfileDTO?> UpdateProfileAsync(Guid userId, UserProfileUpdateDTO dto);
    public Task<bool> DeleteProfileAsync(Guid userId);
}