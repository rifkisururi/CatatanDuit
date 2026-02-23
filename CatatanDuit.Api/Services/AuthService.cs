using System.Security.Claims;
using System.Security.Cryptography;
using CatatanDuit.Api.Data;
using CatatanDuit.Api.Dtos.Auth;
using CatatanDuit.Api.Models;
using CatatanDuit.Api.Security;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CatatanDuit.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        AppDbContext context,
        IPasswordHasher<User> passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            throw new InvalidOperationException("Email already exists");
        }

        var user = new User
        {
            Email = dto.Email,
            Name = dto.Name
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await SeedDefaultCategoriesAsync(user.Id);

        var token = _jwtTokenGenerator.CreateToken(user);

        return new AuthResponseDto
        {
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            },
            Token = token
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, dto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var token = _jwtTokenGenerator.CreateToken(user);

        return new AuthResponseDto
        {
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name
            },
            Token = token
        };
    }

    private async Task SeedDefaultCategoriesAsync(Guid userId)
    {
        var existingCategories = await _context.Categories
            .Where(c => c.UserId == userId)
            .CountAsync();

        if (existingCategories > 0)
        {
            return;
        }

        var incomeCategories = new[]
        {
            "Gaji", "Bonus", "Transfer Masuk", "Lainnya"
        };

        var expenseCategories = new[]
        {
            "Makan & Minum", "Transport", "Belanja", "Tagihan", "Hiburan", "Kesehatan", "Transfer Keluar", "Lainnya"
        };

        foreach (var name in incomeCategories)
        {
            _context.Categories.Add(new Category
            {
                UserId = userId,
                Name = name,
                Type = CategoryType.Income
            });
        }

        foreach (var name in expenseCategories)
        {
            _context.Categories.Add(new Category
            {
                UserId = userId,
                Name = name,
                Type = CategoryType.Expense
            });
        }

        await _context.SaveChangesAsync();
    }
}
