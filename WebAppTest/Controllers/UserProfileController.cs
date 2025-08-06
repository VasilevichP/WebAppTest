using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
[EnableCors("CORSSpecifications")]
public class UserProfileController (IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var profile = await userService.GetProfileAsync(userId);
        return Ok(profile);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdateDTO dto)
    {
        Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var updated = await userService.UpdateProfileAsync(userId, dto);
        return Ok(updated);
    }
}