using System.Net;
using Mapster;
using Microsoft.EntityFrameworkCore;
using WebAppTest.Data;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Exceptions;

namespace WebAppTest.Services;

public class UserService(AppDbContext context) : IUserService
{
    public async Task<List<UserBaseDTO>> GetAllUsers()
    {
        var users = await context.Users.Where(u => u.Role=="User").ToListAsync();
        return users.Adapt<List<UserBaseDTO>>();
    }

    public async Task<List<UserBaseDTO>> GetAllUsersFiltered(FilterUsersDTO filter)
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
        return users.Adapt<List<UserBaseDTO>>();
    }

    public async Task<UserProfileDTO> GetProfileAsync(Guid userId)
    {
        var user = await GetUserAsync(userId)??
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");
        return user.Adapt<UserProfileDTO>();
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
        var user = await GetUserAsync(userId)??
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");

        dto.Adapt(user);
        if(await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.BadRequest, "Возникла ошибка при обновлении профиля");

        return user.Adapt<UserProfileDTO>();
    }

    public async Task DeleteProfileAsync(Guid userId)
    {
        var user = await context.Users.FindAsync(userId) ??
            throw new HttpException(HttpStatusCode.NotFound, "Пользователь не найден");

        context.Users.Remove(user);
        if(await context.SaveChangesAsync() == 0)
            throw new HttpException(HttpStatusCode.BadRequest, "Возникла ошибка при удалении профиля");
    }
}