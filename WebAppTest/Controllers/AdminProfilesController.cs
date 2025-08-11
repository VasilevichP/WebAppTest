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
    public async Task<IActionResult> GetAllProfiles()
    {
        var users = await userService.GetAllUsers();
        return Ok(users);
    }
    
    [HttpPost("filter")]
    public async Task<IActionResult> GetAllFilteredProfiles([FromBody] FilterUsersDTO filter)
    {
        var users = await userService.GetAllUsersFiltered(filter);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProfile(Guid id)
    {
        var profile = await userService.GetProfileAsync(id);
        return Ok(profile);
    }
}