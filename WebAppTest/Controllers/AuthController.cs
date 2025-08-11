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
    public async Task<ActionResult> Register(UserAuthDTO request)
    {
        var user = await authService.RegisterAsync(request);
        return Ok(user);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> Login(UserAuthDTO request)
    {
        var token = await authService.LoginAsync(request);
        return Ok(token);
    }

    [HttpPost("refresh_token")]
    public async Task<ActionResult> RefreshToken(RefreshTokenRequestDTO request)
    {
        var result = await authService.RefreshTokensAsync(request);
        return Ok(result);
    }

}