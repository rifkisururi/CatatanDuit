using System.Security.Claims;
using CatatanDuit.Api.Common;
using CatatanDuit.Api.Dtos.ApiKeys;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatatanDuit.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ApiKeysController : ControllerBase
{
    private readonly IApiKeyService _apiKeyService;

    public ApiKeysController(IApiKeyService apiKeyService)
    {
        _apiKeyService = apiKeyService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ApiKeyDto>>>> GetApiKeys()
    {
        var userId = GetUserId();
        var apiKeys = await _apiKeyService.GetApiKeysAsync(userId);
        return Ok(ApiResponse<IEnumerable<ApiKeyDto>>.Ok(apiKeys));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CreateApiKeyResponseDto>>> CreateApiKey([FromBody] CreateApiKeyRequestDto dto)
    {
        var userId = GetUserId();
        var (apiKeyDto, rawKey) = await _apiKeyService.CreateApiKeyAsync(userId, dto);

        var response = new CreateApiKeyResponseDto
        {
            ApiKey = apiKeyDto,
            RawKey = rawKey
        };

        return Ok(ApiResponse<CreateApiKeyResponseDto>.Ok(response, "API key created. Save the raw key securely as it won't be shown again."));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteApiKey(Guid id)
    {
        var userId = GetUserId();
        var success = await _apiKeyService.DeleteApiKeyAsync(userId, id);

        if (!success)
        {
            return NotFound(ApiResponse<bool>.Fail("API key not found"));
        }

        return Ok(ApiResponse<bool>.Ok(true, "API key deleted"));
    }

    public class CreateApiKeyResponseDto
    {
        public ApiKeyDto ApiKey { get; set; } = null!;
        public string RawKey { get; set; } = string.Empty;
    }
}
