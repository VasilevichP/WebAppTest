using System.Net;
using Microsoft.EntityFrameworkCore;
using WebAppTest.Data;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Enums;
using WebAppTest.Exceptions;

namespace WebAppTest.Services;

public class BookingService(AppDbContext context) : IBookingService
{
    public async Task<List<BookingDTO>> GetBookingsAsync()
    {
        var bookings = await context.Bookings
            .Include(b => b.Quest)
            .Include(b => b.User)
            .ToListAsync();

        return bookings.Select(BookingDTO.ToDTO).ToList();
    }

    public async Task<List<BookingDTO>> GetBookingsFilteredAsync(FilterBookingDTO filter)
    {
        var query = context.Bookings
            .Include(b => b.Quest)
            .Include(b => b.User)
            .AsQueryable();

        if (filter.DateMin.HasValue)
            query = query.Where(b => b.Date >= filter.DateMin.Value);
        if (filter.DateMax.HasValue)
            query = query.Where(b => b.Date <= filter.DateMax.Value);
        if (filter.UserEmail != null)
            query = query.Where(b => b.User.Email.Contains(filter.UserEmail));
        if (filter.QuestTitle != null)
            query = query.Where(b => b.Quest.Title.Contains(filter.QuestTitle));
        if (filter.Status.HasValue)
            query = query.Where(b => b.Status == filter.Status.Value);

        query = query.OrderBy(b => b.Date);

        var bookings = await query.ToListAsync();
        return bookings.Select(BookingDTO.ToDTO).ToList();
    }

    public async Task<BookingDTO?> GetBookingAsync(Guid id)
    {
        var booking = await context.Bookings.Where(u => u.Id == id).FirstOrDefaultAsync();

        return (booking == null) ? null : BookingDTO.ToDTO(booking);
    }

    public async Task<BookingDTO> CreateBookingAsync(BookingCreateDto dto)
    {
        var quest = await context.Quests.FindAsync(dto.QuestId);
        if (quest == null)
            throw new HttpException(HttpStatusCode.NotFound,"Квест не найден");

        var startTime = dto.Time;
        var endTime = startTime.AddMinutes(quest.DurationMinutes + 20);

        var overlappingBookingExists = await context.Bookings
            .Where(b => b.QuestId == dto.QuestId && b.Date == dto.Date)
            .AnyAsync(b =>
                b.Time <= endTime && b.Time.AddMinutes(quest.DurationMinutes + 20) >= startTime && b.Status == BookingStatus.ACTIVE
            );

        if (overlappingBookingExists)
            throw new HttpException(HttpStatusCode.BadRequest, "На это время уже есть бронирование для данного квеста.");

        var booking = FromCreateDTO(dto);
        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        booking.Quest = (await context.Quests.FindAsync(booking.QuestId))!;
        booking.User = (await context.Users.FindAsync(booking.UserId))!;

        return BookingDTO.ToDTO(booking);
    }

    public async Task<BookingDTO?> CancelAsync(Guid id)
    {
        var booking = await context.Bookings
            .Include(b => b.Quest)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (booking == null) return null;
        booking.Status = BookingStatus.CANCELLED;
        await context.SaveChangesAsync();
        
        return BookingDTO.ToDTO(booking);
    }

    public async Task<BookingDTO?> AdminUpdateAsync(Guid id, BookingUpdateDTO dto)
    {
        var booking = await context.Bookings
            .Include(b => b.Quest)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (booking == null) return null;
        FromUpdateDTO(booking, dto);
        await context.SaveChangesAsync();
        
        return BookingDTO.ToDTO(booking);
    }

    // private BookingDTO ToDTO(Booking booking)
    // {
    //     return new BookingDTO
    //     {
    //         Id = booking.Id,
    //         UserEmail = booking.User.Email,
    //         QuestTitle = booking.Quest.Title,
    //         ParticipantsCount = booking.ParticipantsCount,
    //         Date = booking.Date,
    //         Time = booking.Time,
    //         Status = booking.Status
    //     };
    // }

    private Booking FromCreateDTO(BookingCreateDto dto)
    {
        return new Booking()
        {
            UserId = dto.UserId.Value,
            QuestId = dto.QuestId,
            ParticipantsCount = dto.ParticipantsCount,
            Date = dto.Date,
            Time = dto.Time
        };
    }

    private void FromUpdateDTO(Booking booking, BookingUpdateDTO dto)
    {
        if (dto.Date.HasValue) booking.Date = dto.Date.Value;
        if (dto.Time.HasValue) booking.Time = dto.Time.Value;
        if (dto.Participants.HasValue) booking.ParticipantsCount = dto.Participants.Value;
        if (dto.Status.HasValue) booking.Status = dto.Status.Value;
    }
}