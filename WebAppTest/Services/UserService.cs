using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAppTest.Data;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Exceptions;

namespace WebAppTest.Services;

public class UserService(AppDbContext context) : IUserService
{
    public async Task<List<UserBriefDTO>> GetAllUsers()
    {
        var users = await context.Users.Where(u => u.Role=="User").ToListAsync();
        return users.Select(ToBriefDTO).ToList();
    }

    public async Task<List<UserBriefDTO>> GetAllUsersFiltered(FilterUsersDTO filter)
    {
        var query = context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            query = query.Where(u => u.Email.ToLower().Contains(filter.Email.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.Phone))
        {
            query = query.Where(u => u.Phone.Contains(filter.Phone));
        }
        var users = await query
            .ToListAsync();
        return users.Select(ToBriefDTO).ToList();
    }

    public async Task<UserProfileDTO?> GetProfileAsync(Guid userId)
    {
        var user = await GetUserAsync(userId);
        if (user == null) 
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        return ToProfileDTO(user);
    }

    public async Task<UserAdminDTO> GetProfileForAdminAsync(Guid userId)
    {
        var user = await GetUserAsync(userId);
        if (user == null) 
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        return ToProfileForAdminDTO(user);
    }

    private async Task<User?> GetUserAsync(Guid id)
    {
        var user = await context.Users
            .Include(u => u.Bookings).ThenInclude(b => b.Quest)
            .Include(u => u.Reviews).ThenInclude(r => r.Quest)
            .FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<UserProfileDTO?> UpdateProfileAsync(Guid userId, UserProfileUpdateDTO dto)
    {
        var user = await context.Users
            .Include(u => u.Bookings).ThenInclude(b => b.Quest)
            .Include(u => u.Reviews).ThenInclude(r => r.Quest)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) 
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");

        FromUpdateDTO(user, dto);
        await context.SaveChangesAsync();

        return ToProfileDTO(user);
    }

    public async Task<bool> DeleteProfileAsync(Guid userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null) 
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }
    
    private UserProfileDTO ToProfileDTO(User user)
    {
        return new UserProfileDTO
        {
            Email = user.Email,
            Username = user.Username,
            Phone = user.Phone,
            Bookings = user.Bookings.Select(BookingDTO.ToDTO).ToList(),
            Reviews = user.Reviews.Select(ReviewDTO.ToDTO).ToList()
        };
    }
    
    private UserAdminDTO ToProfileForAdminDTO(User user)
    {
        return new UserAdminDTO
        {
            UserId = user.Id,
            Email = user.Email,
            Username = user.Username,
            Phone = user.Phone,
            Bookings = user.Bookings.Select(ToUserBookingDTO).ToList(),
            Reviews = new List<Review>(user.Reviews)
        };
    }
    
    private UserBriefDTO ToBriefDTO(User user)
    {
        return new UserBriefDTO()
        {
            UserId = user.Id,
            Email = user.Email,
            Username = user.Username,
            Phone = user.Phone
        };
    }
    
    private BookingDTO ToUserBookingDTO(Booking booking)
    {
        return new BookingDTO()
        {
            Id = booking.Id,
            QuestTitle = booking.Quest.Title,
            UserEmail = booking.User.Email,
            Date = booking.Date,
            Time = booking.Time,
            ParticipantsCount = booking.ParticipantsCount,
            Status = booking.Status
        };
    }
    
    private void FromUpdateDTO(User user, UserProfileUpdateDTO dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.NewUsername))
            user.Username = dto.NewUsername;


        if (!string.IsNullOrWhiteSpace(dto.NewPhoneNumber))
            user.Phone = dto.NewPhoneNumber;
        
        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
            user.PasswordHash = new PasswordHasher<User>()
                .HashPassword(user, dto.NewPassword);
    }
}