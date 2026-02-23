using System.Security.Cryptography;
using System.Text;
using CatatanDuit.Api.Data;
using CatatanDuit.Api.Dtos.ApiKeys;
using CatatanDuit.Api.Models;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatatanDuit.Api.Services;

public class ApiKeyService : IApiKeyService
{
    private readonly AppDbContext _context;

    public ApiKeyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ApiKeyDto>> GetApiKeysAsync(Guid userId)
    {
        return await _context.ApiKeys
            .Where(a => a.UserId == userId)
            .Select(a => new ApiKeyDto
            {
                Id = a.Id,
                Label = a.Label,
                IsActive = a.IsActive,
                CreatedAt = a.CreatedAt
            })
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<(ApiKeyDto dto, string rawKey)> CreateApiKeyAsync(Guid userId, CreateApiKeyRequestDto dto)
    {
        var rawKey = GenerateApiKey();
        var keyHash = ComputeHash(rawKey);

        var apiKey = new ApiKey
        {
            UserId = userId,
            KeyHash = keyHash,
            Label = dto.Label,
            IsActive = true
        };

        _context.ApiKeys.Add(apiKey);
        await _context.SaveChangesAsync();

        var apiKeyDto = new ApiKeyDto
        {
            Id = apiKey.Id,
            Label = apiKey.Label,
            IsActive = apiKey.IsActive,
            CreatedAt = apiKey.CreatedAt
        };

        return (apiKeyDto, rawKey);
    }

    public async Task<bool> DeleteApiKeyAsync(Guid userId, Guid apiKeyId)
    {
        var apiKey = await _context.ApiKeys
            .FirstOrDefaultAsync(a => a.Id == apiKeyId && a.UserId == userId);

        if (apiKey == null)
        {
            return false;
        }

        _context.ApiKeys.Remove(apiKey);
        await _context.SaveChangesAsync();

        return true;
    }

    private string GenerateApiKey()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new StringBuilder(32);

        for (int i = 0; i < 32; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }

        return $"cd_{result.ToString()}";
    }

    private string ComputeHash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
