using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using invoice_backend.Data;
using invoice_backend.DTOs.Auth;
using invoice_backend.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace invoice_backend.Services;

public class AuthService(
    ApplicationDbContext context,
    IPasswordHasher<User> passwordHasher,
    IConfiguration configuration)
{
    public async Task<AuthResponseDto> Register(RegisterUserDto request)
    {
        var exist = await context.Users.FirstOrDefaultAsync(user => user.Email == request.Email);
        if (exist is not null) throw new InvalidOperationException("Email already exists");
        var user = new User
        {
            UserName = request.UserName,
            Email = request.Email
        };

        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var tokenValue = GenerateToken(user);


        var newUser = new AuthResponseDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Token = tokenValue
        };
        return newUser;
    }

    public async Task<AuthResponseDto> Login(LoginDto dto)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Email == dto.Email);
        if (user is null) throw new InvalidOperationException("Invalid email or password");

        var hashedPassword = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (hashedPassword is not PasswordVerificationResult.Success)
            throw new InvalidOperationException("Invalid email or password");

        var tokenValue = GenerateToken(user);


        var newUser = new AuthResponseDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Token = tokenValue
        };

        return newUser;
    }

    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName)
        };

        var secretKey = configuration.GetSection("jwt:SecretKey").Value;
        var issuer = configuration.GetSection("jwt:Issuer").Value;
        var audience = configuration.GetSection("jwt:Audience").Value;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}