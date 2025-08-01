using WebAppTest.DTO;
using WebAppTest.Entities;

namespace WebAppTest.Services;

public interface IAuthService
{
    Task<User?> RegisterAsync(UserAuthDTO request);
    Task<TokenResponseDTO> LoginAsync(UserAuthDTO request);
    Task<TokenResponseDTO> RefreshTokensAsync(RefreshTokenRequestDTO request);
}