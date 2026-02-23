using CatatanDuit.Api.Dtos.Categories;

namespace CatatanDuit.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync(Guid userId);
    Task<CategoryDto?> GetCategoryByIdAsync(Guid userId, Guid categoryId);
    Task<CategoryDto> CreateCategoryAsync(Guid userId, CreateCategoryRequestDto dto);
    Task<CategoryDto?> UpdateCategoryAsync(Guid userId, Guid categoryId, UpdateCategoryRequestDto dto);
    Task<bool> DeleteCategoryAsync(Guid userId, Guid categoryId);
}
