using CatatanDuit.Api.Data;
using CatatanDuit.Api.Dtos.Categories;
using CatatanDuit.Api.Models;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatatanDuit.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(Guid userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == userId)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type
            })
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid userId, Guid categoryId)
    {
        return await _context.Categories
            .Where(c => c.Id == categoryId && c.UserId == userId)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoryDto> CreateCategoryAsync(Guid userId, CreateCategoryRequestDto dto)
    {
        var category = new Category
        {
            UserId = userId,
            Name = dto.Name,
            Type = dto.Type
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type
        };
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(Guid userId, Guid categoryId, UpdateCategoryRequestDto dto)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

        if (category == null)
        {
            return null;
        }

        category.Name = dto.Name;
        category.Type = dto.Type;
        category.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type
        };
    }

    public async Task<bool> DeleteCategoryAsync(Guid userId, Guid categoryId)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

        if (category == null)
        {
            return false;
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }
}
