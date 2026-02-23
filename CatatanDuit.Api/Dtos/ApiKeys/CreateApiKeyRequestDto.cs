using System.ComponentModel.DataAnnotations;

namespace CatatanDuit.Api.Dtos.ApiKeys;

public class CreateApiKeyRequestDto
{
    [StringLength(256)]
    public string? Label { get; set; }
}
