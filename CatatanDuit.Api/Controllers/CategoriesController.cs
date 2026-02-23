using System.Security.Claims;
using CatatanDuit.Api.Common;
using CatatanDuit.Api.Dtos.Categories;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatatanDuit.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetCategories()
    {
        var userId = GetUserId();
        var categories = await _categoryService.GetCategoriesAsync(userId);
        return Ok(ApiResponse<IEnumerable<CategoryDto>>.Ok(categories));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategory(Guid id)
    {
        var userId = GetUserId();
        var category = await _categoryService.GetCategoryByIdAsync(userId, id);

        if (category == null)
        {
            return NotFound(ApiResponse<CategoryDto>.Fail("Category not found"));
        }

        return Ok(ApiResponse<CategoryDto>.Ok(category));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory([FromBody] CreateCategoryRequestDto dto)
    {
        var userId = GetUserId();
        var category = await _categoryService.CreateCategoryAsync(userId, dto);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, ApiResponse<CategoryDto>.Ok(category, "Category created"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CategoryDto>>> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequestDto dto)
    {
        var userId = GetUserId();
        var category = await _categoryService.UpdateCategoryAsync(userId, id, dto);

        if (category == null)
        {
            return NotFound(ApiResponse<CategoryDto>.Fail("Category not found"));
        }

        return Ok(ApiResponse<CategoryDto>.Ok(category, "Category updated"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCategory(Guid id)
    {
        var userId = GetUserId();
        var success = await _categoryService.DeleteCategoryAsync(userId, id);

        if (!success)
        {
            return NotFound(ApiResponse<bool>.Fail("Category not found"));
        }

        return Ok(ApiResponse<bool>.Ok(true, "Category deleted"));
    }
}
