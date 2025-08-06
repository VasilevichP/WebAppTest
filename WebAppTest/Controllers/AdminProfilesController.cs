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
public class AdminProfilesController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAllUsers();
        return Ok(users);
    }
    
    [HttpPost("filter")]
    public async Task<IActionResult> GetAllFiltered([FromBody] FilterUsersDTO filter)
    {
        var users = await userService.GetAllUsersFiltered(filter);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(Guid id)
    {
        Console.WriteLine((id));
        var profile = await userService.GetProfileForAdminAsync(id);
        return Ok(profile);
    }
}