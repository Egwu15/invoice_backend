using invoice_backend.Data;
using invoice_backend.DTOs.Auth;
using invoice_backend.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace invoice_backend.Services;

public class AuthService(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
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
        var newUser = new AuthResponseDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };
        return newUser;
    }
}