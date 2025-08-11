using WebAppTest.DTO;
using WebAppTest.Entities;

namespace WebAppTest.Services;

public interface IBookingService
{
    public Task<List<BookingDTO>> GetBookingsAsync();
    public Task<List<BookingDTO>> GetBookingsFilteredAsync(FilterBookingDTO filter);
    public Task<BookingDTO> GetBookingAsync(Guid id);
    public Task<BookingDTO> CreateBookingAsync(BookingCreateDto booking);
    public Task<BookingDTO> CancelAsync(Guid id);
    public Task<BookingDTO> UpdateAsync(Guid id, BookingUpdateDTO dto);
    public Task DeleteAsync(Guid id);
}