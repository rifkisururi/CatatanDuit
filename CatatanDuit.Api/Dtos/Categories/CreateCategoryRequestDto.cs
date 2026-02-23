using System.ComponentModel.DataAnnotations;
using CatatanDuit.Api.Models;

namespace CatatanDuit.Api.Dtos.Categories;

public class CreateCategoryRequestDto
{
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public CategoryType Type { get; set; }
}
