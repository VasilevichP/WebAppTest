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
        return users.Select(UserBriefDTO.ToDTO).ToList();
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
        return users.Select(UserBriefDTO.ToDTO).ToList();
    }

    public async Task<UserProfileDTO?> GetProfileAsync(Guid userId)
    {
        var user = await GetUserAsync(userId);
        if (user == null) 
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        return UserProfileDTO.ToDTO(user);
    }

    public async Task<UserAdminDTO> GetProfileForAdminAsync(Guid userId)
    {
        var user = await GetUserAsync(userId);
        if (user == null) 
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        return UserAdminDTO.ToDTO(user);
    }

    private async Task<User?> GetUserAsync(Guid id)
    {
        var user = await context.Users
            .Include(u => u.Bookings).ThenInclude(b => b.Quest)
            .Include(u => u.Reviews).ThenInclude(r => r.Quest)
            .FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<UserProfileDTO> UpdateProfileAsync(Guid userId, UserProfileUpdateDTO dto)
    {
        var user = await context.Users
            .Include(u => u.Bookings).ThenInclude(b => b.Quest)
            .Include(u => u.Reviews).ThenInclude(r => r.Quest)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) 
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");

        UserProfileUpdateDTO.FromDTO(user, dto);
        if(await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.BadRequest, "Возникла ошибка при обновлении профиля");

        return UserProfileDTO.ToDTO(user);
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
}