using System.Net;
using System.Runtime.InteropServices.JavaScript;
using Mapster;
using Microsoft.EntityFrameworkCore;
using WebAppTest.Data;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Enums;
using WebAppTest.Exceptions;

namespace WebAppTest.Services;

public class BookingService(AppDbContext context) : IBookingService
{
    private async Task<Booking?> FindBookingAsync(Guid id)
    {
        var booking = await context.Bookings
            .Include(b => b.Quest)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
        return booking;
    }

    public async Task<List<BookingDTO>> GetBookingsAsync()
    {
        var bookings = await context.Bookings
            .Include(b => b.Quest)
            .Include(b => b.User)
            .ToListAsync();

        return bookings.Adapt<List<BookingDTO>>();
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
        return bookings.Adapt<List<BookingDTO>>();
    }

    public async Task<BookingDTO> GetBookingAsync(Guid id)
    {
        var booking = await FindBookingAsync(id) ??
                      throw new HttpException(HttpStatusCode.NotFound, "Бронирование не найдено");
        return booking.Adapt<BookingDTO>();
    }

    public async Task<BookingDTO> CreateBookingAsync(BookingCreateDto dto)
    {
        if (dto.Date<DateOnly.FromDateTime(DateTime.Today))
            throw new HttpException(HttpStatusCode.BadRequest, "Выбрана дата раньше сегодняшней");
        var quest = await context.Quests.FindAsync(dto.QuestId) ??
                    throw new HttpException(HttpStatusCode.NotFound, "Квест не найден");
        var user = (await context.Users.FindAsync(dto.UserId)) ??
                   throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        if (quest.MaxParticipants < dto.Participants)
            throw new HttpException(HttpStatusCode.BadRequest,
                $"Количество участников не должно превышать {quest.MaxParticipants} человек");

        var startTime = dto.Time;
        var endTime = startTime.AddMinutes(quest.DurationMinutes + 20);

        var overlappingBookingExists = await context.Bookings
            .Where(b => b.QuestId == dto.QuestId && b.Date == dto.Date)
            .AnyAsync(b =>
                b.Time <= endTime && b.Time.AddMinutes(quest.DurationMinutes + 20) >= startTime &&
                b.Status == BookingStatus.ACTIVE
            );

        if (overlappingBookingExists)
            throw new HttpException(HttpStatusCode.BadRequest,
                "На это время уже есть бронирование для данного квеста.");

        var booking = dto.Adapt<Booking>();
        context.Bookings.Add(booking);
        if (await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.InternalServerError, "Возникла ошибка при бронировании");

        booking.Quest = quest;
        booking.User = user;

        return booking.Adapt<BookingDTO>();
    }

    public async Task<BookingDTO> CancelAsync(Guid id)
    {
        var booking = await FindBookingAsync(id) ??
                      throw new HttpException(HttpStatusCode.NotFound, "Бронирование не найдено");
        booking.Status = BookingStatus.CANCELLED;
        if (await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.InternalServerError, "Возникла ошибка при отмене бронирования");

        return booking.Adapt<BookingDTO>();
    }

    public async Task<BookingDTO> UpdateAsync(Guid id, BookingUpdateDTO dto)
    {
        var booking = await FindBookingAsync(id) ??
            throw new HttpException(HttpStatusCode.NotFound, "Бронирование не найдено");
        dto.Adapt<Booking>();
        if (await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.InternalServerError, "Возникла ошибка изменении записи");

        return booking.Adapt<BookingDTO>();
    }

    public async Task DeleteAsync(Guid id)
    {
        var booking = await FindBookingAsync(id) ??
                      throw new HttpException(HttpStatusCode.NotFound, "Бронирование не найдено");
        context.Bookings.Remove(booking);
        if (await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.InternalServerError, "Возникла ошибка при удалении");
    }
}