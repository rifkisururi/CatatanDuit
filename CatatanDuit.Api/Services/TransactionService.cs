using CatatanDuit.Api.Common;
using CatatanDuit.Api.Data;
using CatatanDuit.Api.Dtos.Transactions;
using CatatanDuit.Api.Models;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatatanDuit.Api.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;

    public TransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<TransactionDto>> GetTransactionsAsync(
        Guid userId,
        Guid? walletId,
        Guid? categoryId,
        TransactionType? type,
        DateTime? dateFrom,
        DateTime? dateTo,
        int page,
        int pageSize)
    {
        var query = _context.Transactions
            .Include(t => t.Wallet)
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        if (walletId.HasValue)
        {
            query = query.Where(t => t.WalletId == walletId.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId.Value);
        }

        if (type.HasValue)
        {
            query = query.Where(t => t.Type == type.Value);
        }

        if (dateFrom.HasValue)
        {
            query = query.Where(t => t.Date >= dateFrom.Value);
        }

        if (dateTo.HasValue)
        {
            query = query.Where(t => t.Date <= dateTo.Value);
        }

        var totalCount = await query.CountAsync();

        var transactions = await query
            .OrderByDescending(t => t.Date)
            .ThenByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                WalletId = t.WalletId,
                WalletName = t.Wallet.Name,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                Amount = t.Amount,
                Type = t.Type,
                Description = t.Description,
                Date = t.Date,
                Source = t.Source
            })
            .ToListAsync();

        return new PagedResult<TransactionDto>(transactions, page, pageSize, totalCount);
    }

    public async Task<TransactionDto?> GetTransactionByIdAsync(Guid userId, Guid transactionId)
    {
        return await _context.Transactions
            .Include(t => t.Wallet)
            .Include(t => t.Category)
            .Where(t => t.Id == transactionId && t.UserId == userId)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                WalletId = t.WalletId,
                WalletName = t.Wallet.Name,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                Amount = t.Amount,
                Type = t.Type,
                Description = t.Description,
                Date = t.Date,
                Source = t.Source
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TransactionDto> CreateTransactionAsync(Guid userId, CreateTransactionRequestDto dto)
    {
        var wallet = await _context.Wallets
            .FirstOrDefaultAsync(w => w.Id == dto.WalletId && w.UserId == userId);

        if (wallet == null)
        {
            throw new InvalidOperationException("Wallet not found");
        }

        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.UserId == userId);

        if (category == null)
        {
            throw new InvalidOperationException("Category not found");
        }

        var transaction = new Transaction
        {
            UserId = userId,
            WalletId = dto.WalletId,
            CategoryId = dto.CategoryId,
            Amount = dto.Amount,
            Type = dto.Type,
            Description = dto.Description,
            Date = dto.Date,
            Source = TransactionSource.Web
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return new TransactionDto
        {
            Id = transaction.Id,
            WalletId = transaction.WalletId,
            WalletName = wallet.Name,
            CategoryId = transaction.CategoryId,
            CategoryName = category.Name,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description,
            Date = transaction.Date,
            Source = transaction.Source
        };
    }

    public async Task<TransactionDto?> UpdateTransactionAsync(Guid userId, Guid transactionId, UpdateTransactionRequestDto dto)
    {
        var transaction = await _context.Transactions
            .Include(t => t.Wallet)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);

        if (transaction == null)
        {
            return null;
        }

        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId && c.UserId == userId);

        if (category == null)
        {
            throw new InvalidOperationException("Category not found");
        }

        transaction.CategoryId = dto.CategoryId;
        transaction.Amount = dto.Amount;
        transaction.Type = dto.Type;
        transaction.Description = dto.Description;
        transaction.Date = dto.Date;
        transaction.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return new TransactionDto
        {
            Id = transaction.Id,
            WalletId = transaction.WalletId,
            WalletName = transaction.Wallet.Name,
            CategoryId = transaction.CategoryId,
            CategoryName = category.Name,
            Amount = transaction.Amount,
            Type = transaction.Type,
            Description = transaction.Description,
            Date = transaction.Date,
            Source = transaction.Source
        };
    }

    public async Task<bool> DeleteTransactionAsync(Guid userId, Guid transactionId)
    {
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);

        if (transaction == null)
        {
            return false;
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return true;
    }
}
