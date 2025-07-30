using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserAuthDTO request)
    {
        var user = await authService.RegisterAsync(request);
        if (user == null)
            return BadRequest("Пользователь с таким именем уже существует");

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDTO>> Login(UserAuthDTO request)
    {
        var result = await authService.LoginAsync(request);
        if (result == null)
            return BadRequest("Неправильный логин и/или пароль");

        return Ok(result);
    }

    [HttpPost("refresh_token")]
    public async Task<ActionResult<TokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO request)
    {
        var result = await authService.RefreshTokensAsync(request);
        if (result is null || result.AccessToken is null || result.RefreshToken is null)
            return Unauthorized("Возникла ошибка при обновлении токена");

        return Ok(result);
    }

    [Authorize]
    [HttpGet]
    public IActionResult AuthOnly()
    {
        return Ok("u authed. good boi");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult AdminOnly()
    {
        return Ok("u admin bitch");
    }
}