namespace CatatanDuit.Api.Dtos.Auth;

public class AuthResponseDto
{
    public UserDto User { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Name { get; set; }
}
