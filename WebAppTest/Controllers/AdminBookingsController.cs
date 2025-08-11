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
public class AdminBookingsController(IBookingService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBookings()
    {
        var bookings = await service.GetBookingsAsync();
        return Ok(bookings);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetFilteredBookings([FromBody] FilterBookingDTO filter)
    {
        var bookings = await service.GetBookingsFilteredAsync(filter);
        return Ok(bookings);
    }
    
    [HttpPatch]
    public async Task<IActionResult> CancelBooking(Guid id)
    {
        var booking = await service.CancelAsync(id);
        return Ok(booking);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateBooking(Guid id, [FromBody] BookingUpdateDTO dto)
    {
        var booking = await service.UpdateAsync(id, dto);
        return Ok(booking);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
        await service.DeleteAsync(id);
        return Ok();
    }

}