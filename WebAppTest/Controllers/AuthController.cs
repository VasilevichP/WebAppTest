using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableCors("CORSSpecifications")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserAuthDTO request)
    {
        var user = await authService.RegisterAsync(request);
        return Ok(user);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDTO>> Login(UserAuthDTO request)
    {
        var result = await authService.LoginAsync(request);
        return Ok(result);
    }

    [HttpPost("refresh_token")]
    public async Task<ActionResult<TokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO request)
    {
        var result = await authService.RefreshTokensAsync(request);
        return Ok(result);
    }

}