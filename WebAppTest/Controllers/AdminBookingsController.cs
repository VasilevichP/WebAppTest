using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppTest.DTO;
using WebAppTest.Services;

namespace WebAppTest.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminBookingsController(IBookingService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var bookings = await service.GetBookingsAsync();
        return (bookings.Any())? Ok(bookings):Ok("Список бронирований пуст");
    }
    
    [HttpPost("filter")]
    public async Task<IActionResult> GetFiltered([FromBody] FilterBookingDTO filter)
    {
        var bookings = await service.GetBookingsFilteredAsync(filter);
        return (bookings.Any())? Ok(bookings):Ok("Не найдено подходящих записей");
    }
    
    [HttpPatch("cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var booking = await service.CancelAsync(id);
        return (booking == null) ? NotFound("Запись не найдена") : Ok(booking);
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] BookingUpdateDTO dto)
    {
        var booking = await service.AdminUpdateAsync(id, dto);
        return (booking == null) ? NotFound("Запись не найдена") : Ok(booking);
    }
}