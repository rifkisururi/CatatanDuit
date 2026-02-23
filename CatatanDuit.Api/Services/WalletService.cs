using CatatanDuit.Api.Data;
using CatatanDuit.Api.Dtos.Wallets;
using CatatanDuit.Api.Models;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatatanDuit.Api.Services;

public class WalletService : IWalletService
{
    private readonly AppDbContext _context;

    public WalletService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WalletDto>> GetWalletsAsync(Guid userId)
    {
        var wallets = await _context.Wallets
            .Where(w => w.UserId == userId)
            .ToListAsync();

        var result = new List<WalletDto>();

        foreach (var wallet in wallets)
        {
            var currentBalance = await CalculateCurrentBalanceAsync(wallet.Id);
            result.Add(new WalletDto
            {
                Id = wallet.Id,
                Name = wallet.Name,
                Type = wallet.Type,
                AccountNumber = wallet.AccountNumber,
                InitialBalance = wallet.InitialBalance,
                CurrentBalance = currentBalance
            });
        }

        return result;
    }

    public async Task<WalletDto?> GetWalletByIdAsync(Guid userId, Guid walletId)
    {
        var wallet = await _context.Wallets
            .FirstOrDefaultAsync(w => w.Id == walletId && w.UserId == userId);

        if (wallet == null)
        {
            return null;
        }

        var currentBalance = await CalculateCurrentBalanceAsync(wallet.Id);

        return new WalletDto
        {
            Id = wallet.Id,
            Name = wallet.Name,
            Type = wallet.Type,
            AccountNumber = wallet.AccountNumber,
            InitialBalance = wallet.InitialBalance,
            CurrentBalance = currentBalance
        };
    }

    public async Task<WalletDto> CreateWalletAsync(Guid userId, CreateWalletRequestDto dto)
    {
        var wallet = new Wallet
        {
            UserId = userId,
            Name = dto.Name,
            Type = dto.Type,
            AccountNumber = dto.AccountNumber,
            InitialBalance = dto.InitialBalance
        };

        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();

        return new WalletDto
        {
            Id = wallet.Id,
            Name = wallet.Name,
            Type = wallet.Type,
            AccountNumber = wallet.AccountNumber,
            InitialBalance = wallet.InitialBalance,
            CurrentBalance = wallet.InitialBalance
        };
    }

    public async Task<WalletDto?> UpdateWalletAsync(Guid userId, Guid walletId, UpdateWalletRequestDto dto)
    {
        var wallet = await _context.Wallets
            .FirstOrDefaultAsync(w => w.Id == walletId && w.UserId == userId);

        if (wallet == null)
        {
            return null;
        }

        wallet.Name = dto.Name;
        wallet.Type = dto.Type;
        wallet.AccountNumber = dto.AccountNumber;
        wallet.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        var currentBalance = await CalculateCurrentBalanceAsync(wallet.Id);

        return new WalletDto
        {
            Id = wallet.Id,
            Name = wallet.Name,
            Type = wallet.Type,
            AccountNumber = wallet.AccountNumber,
            InitialBalance = wallet.InitialBalance,
            CurrentBalance = currentBalance
        };
    }

    public async Task<bool> DeleteWalletAsync(Guid userId, Guid walletId)
    {
        var wallet = await _context.Wallets
            .FirstOrDefaultAsync(w => w.Id == walletId && w.UserId == userId);

        if (wallet == null)
        {
            return false;
        }

        _context.Wallets.Remove(wallet);
        await _context.SaveChangesAsync();

        return true;
    }

    private async Task<decimal> CalculateCurrentBalanceAsync(Guid walletId)
    {
        var wallet = await _context.Wallets
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == walletId);

        if (wallet == null)
        {
            return 0;
        }

        var transactions = await _context.Transactions
            .Where(t => t.WalletId == walletId)
            .ToListAsync();

        var totalCredit = transactions.Where(t => t.Type == TransactionType.Credit).Sum(t => t.Amount);
        var totalDebit = transactions.Where(t => t.Type == TransactionType.Debit).Sum(t => t.Amount);

        return wallet.InitialBalance + totalCredit - totalDebit;
    }
}
