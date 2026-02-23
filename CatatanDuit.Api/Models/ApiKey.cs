namespace CatatanDuit.Api.Models;

public class ApiKey : BaseEntity
{
    public Guid UserId { get; set; }
    public string KeyHash { get; set; } = string.Empty;
    public string? Label { get; set; }
    public bool IsActive { get; set; } = true;

    public User User { get; set; } = null!;
}
