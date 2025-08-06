using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAppTest.Data;
using WebAppTest.DTO;
using WebAppTest.Entities;
using WebAppTest.Exceptions;

namespace WebAppTest.Services;

public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
{
    public async Task<UserBriefDTO> RegisterAsync(UserAuthDTO request)
    {
        if (await context.Users.AnyAsync((u => u.Email == request.Email)))
        {
            throw new HttpException(HttpStatusCode.BadRequest,"Данная почта уже используется");
        }
        var user = new User();
        var hashedPassword = new PasswordHasher<User>()
            .HashPassword(user, request.Password);
        user.Email = request.Email;
        user.Username = request.Email;
        user.PasswordHash = hashedPassword;
        user.Role = (await context.Users.AnyAsync(u => u.Role == "Admin")? "User" : "Admin");
         context.Users.Add(user);
         try
         {
             var x = await context.SaveChangesAsync();
         }
         catch (DbUpdateException)
         {
            throw new HttpException(HttpStatusCode.BadRequest,"Возникла ошибка при регистрации");   
         }
        return UserBriefDTO.ToDTO(user);
    }

    public async Task<TokenResponseDTO> LoginAsync(UserAuthDTO request)
    {
        var user = await context.Users.FirstOrDefaultAsync((u => u.Email == request.Email));
        if (user is null)
        {
            throw new HttpException(HttpStatusCode.BadRequest,"Неправильный логин и/или пароль");
        }

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
            == PasswordVerificationResult.Failed)
        {
            throw new HttpException(HttpStatusCode.BadRequest,"Неправильный логин и/или пароль");
        }

        return await CreateTokenResponseDto(user);
    }

    private async Task<TokenResponseDTO> CreateTokenResponseDto(User user)
    {
        return new TokenResponseDTO()
        {
            AccessToken = CreateToken(user),
            RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
        };
    }

    public async Task<TokenResponseDTO> RefreshTokensAsync(RefreshTokenRequestDTO request)
    {
        var user = await ValidateRefreshTokenAsync(request.UserId,request.RefreshToken);
        if (user is null)
        {
            throw new HttpException(HttpStatusCode.BadRequest,"Пользователь не найден");
        }

        return await CreateTokenResponseDto(user);
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
            
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("AppSettings:Issuer"),
            audience: configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.Now.AddHours(4),
            signingCredentials: creds
        );
            
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    private string GenerateRefreshToken()
    {
        var rn = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(rn);
        return Convert.ToBase64String(rn);
    }

    private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddHours(4);
        await context.SaveChangesAsync();
        return refreshToken; 
    }

    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is null || refreshToken != user.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new HttpException(HttpStatusCode.BadRequest,"Возникла ошибка при обновлении токена");
        }
        
        return user;
    }
}