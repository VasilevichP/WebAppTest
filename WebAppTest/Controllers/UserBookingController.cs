using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
[EnableCors("CORSSpecifications")]
public class UserBookingController(IBookingService service):ControllerBase
{
    [HttpPost("book")]
    public async Task<IActionResult> Book(BookingCreateDto dto)
    {
        dto.UserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var booking = await service.CreateBookingAsync(dto);
        return Ok(booking);
    }
    
    [HttpPatch("cancel")]
    public async Task<IActionResult> Cancel(Guid id, BookingUpdateDTO dto)
    {
        var booking = await service.AdminUpdateAsync(id,dto);
        return (booking==null)? NotFound("Бронирование не найдено"):Ok(booking);
    }
}