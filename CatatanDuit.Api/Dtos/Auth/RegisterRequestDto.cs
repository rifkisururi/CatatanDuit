using System.ComponentModel.DataAnnotations;

namespace CatatanDuit.Api.Dtos.Auth;

public class RegisterRequestDto
{
    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [StringLength(256)]
    public string? Name { get; set; }
}
