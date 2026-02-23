using CatatanDuit.Api.Dtos.ApiKeys;

namespace CatatanDuit.Api.Services.Interfaces;

public interface IApiKeyService
{
    Task<IEnumerable<ApiKeyDto>> GetApiKeysAsync(Guid userId);
    Task<(ApiKeyDto dto, string rawKey)> CreateApiKeyAsync(Guid userId, CreateApiKeyRequestDto dto);
    Task<bool> DeleteApiKeyAsync(Guid userId, Guid apiKeyId);
}
