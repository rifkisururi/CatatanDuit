using CatatanDuit.Api.Dtos.Wallets;

namespace CatatanDuit.Api.Services.Interfaces;

public interface IWalletService
{
    Task<IEnumerable<WalletDto>> GetWalletsAsync(Guid userId);
    Task<WalletDto?> GetWalletByIdAsync(Guid userId, Guid walletId);
    Task<WalletDto> CreateWalletAsync(Guid userId, CreateWalletRequestDto dto);
    Task<WalletDto?> UpdateWalletAsync(Guid userId, Guid walletId, UpdateWalletRequestDto dto);
    Task<bool> DeleteWalletAsync(Guid userId, Guid walletId);
}
