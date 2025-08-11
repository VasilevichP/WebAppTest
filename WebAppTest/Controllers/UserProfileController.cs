using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Exceptions;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
[EnableCors("CORSSpecifications")]
public class UserProfileController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                               throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден"));
        var profile = await userService.GetProfileAsync(userId);
        return Ok(profile);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdateDTO dto)
    {
        Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                               throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден"));
        var updated = await userService.UpdateProfileAsync(userId, dto);
        return Ok(updated);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProfile()
    {
        Guid userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                               throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден"));
        await userService.DeleteProfileAsync(userId);
        return Ok();
    }
}