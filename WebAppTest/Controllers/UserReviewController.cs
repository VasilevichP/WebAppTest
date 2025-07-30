using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class UserReviewController(IReviewService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDTO dto)
    {
        dto.UserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var review = await service.CreateReviewAsync(dto);
        return Ok(review);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteReview(Guid id)
    {
        var isDeleted = await service.DeleteReviewAsync(id);
        return isDeleted ? Ok("Отзыв был удален") : NotFound("Отзыв не найден");
    }
}