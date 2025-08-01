using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
[EnableCors("CORSSpecifications")]
public class AdminProfileController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAllUsers();
        return users.Any() ? Ok(users) : Ok("Список пользователей пуст");
    }
    
    [HttpPost("filter")]
    public async Task<IActionResult> GetAllFiltered([FromBody] FilterUsersDTO filter)
    {
        var users = await userService.GetAllUsersFiltered(filter);
        return users.Any() ? Ok(users) : Ok("Не найдено подходящих пользователей");
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(Guid userId)
    {
        var profile = await userService.GetProfileForAdminAsync(userId);
        return (profile == null) ? NotFound("Профиль пользователя не найден") : Ok(profile);
    }
}