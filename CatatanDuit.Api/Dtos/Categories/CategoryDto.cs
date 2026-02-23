using CatatanDuit.Api.Models;

namespace CatatanDuit.Api.Dtos.Categories;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
}
