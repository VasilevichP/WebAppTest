using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
public class UserBookingController(IBookingService service):ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateBooking(BookingCreateDto dto)
    {
        dto.UserId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier) ??  
            throw new HttpException(HttpStatusCode.NotFound,"Пользователь не найден"));
        var booking = await service.CreateBookingAsync(dto);
        return Ok(booking);
    }
    
    [HttpPatch]
    public async Task<IActionResult> CancelBooking(Guid id)
    {
        var booking = await service.CancelAsync(id);
        return Ok(booking);
    }
}