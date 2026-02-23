namespace CatatanDuit.Api.Dtos.ApiKeys;

public class ApiKeyDto
{
    public Guid Id { get; set; }
    public string? Label { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
