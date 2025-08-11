using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Exceptions;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
[EnableCors("CORSSpecifications")]
public class UserReviewController(IReviewService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDTO dto)
    {
        dto.UserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)??  
                              throw new HttpException(HttpStatusCode.NotFound,"Пользователь не найден"));
        var review = await service.CreateReviewAsync(dto);
        return Ok(review);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteReview(Guid id)
    {
        await service.DeleteReviewAsync(id);
        return Ok();
    }
}