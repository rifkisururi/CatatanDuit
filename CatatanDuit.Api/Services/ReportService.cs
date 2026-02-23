using CatatanDuit.Api.Data;
using CatatanDuit.Api.Dtos.Reports;
using CatatanDuit.Api.Models;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatatanDuit.Api.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WalletSummaryDto>> GetSummaryAsync(Guid userId)
    {
        var wallets = await _context.Wallets
            .Where(w => w.UserId == userId)
            .ToListAsync();

        var result = new List<WalletSummaryDto>();

        foreach (var wallet in wallets)
        {
            var transactions = await _context.Transactions
                .Where(t => t.WalletId == wallet.Id)
                .ToListAsync();

            var totalIncome = transactions
                .Where(t => t.Type == TransactionType.Credit)
                .Sum(t => t.Amount);

            var totalExpense = transactions
                .Where(t => t.Type == TransactionType.Debit)
                .Sum(t => t.Amount);

            var currentBalance = wallet.InitialBalance + totalIncome - totalExpense;

            result.Add(new WalletSummaryDto
            {
                WalletId = wallet.Id,
                WalletName = wallet.Name,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                CurrentBalance = currentBalance
            });
        }

        return result;
    }

    public async Task<IEnumerable<MonthlyReportItemDto>> GetMonthlyReportAsync(Guid userId, int year)
    {
        var transactions = await _context.Transactions
            .Where(t => t.UserId == userId && t.Date.Year == year)
            .ToListAsync();

        var monthlyData = new List<MonthlyReportItemDto>();

        for (int month = 1; month <= 12; month++)
        {
            var monthTransactions = transactions
                .Where(t => t.Date.Month == month)
                .ToList();

            var totalIncome = monthTransactions
                .Where(t => t.Type == TransactionType.Credit)
                .Sum(t => t.Amount);

            var totalExpense = monthTransactions
                .Where(t => t.Type == TransactionType.Debit)
                .Sum(t => t.Amount);

            monthlyData.Add(new MonthlyReportItemDto
            {
                Month = month,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                NetBalance = totalIncome - totalExpense
            });
        }

        return monthlyData;
    }

    public async Task<IEnumerable<CategoryReportItemDto>> GetByCategoryAsync(Guid userId, Guid? walletId, int? month, int? year)
    {
        var query = _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId);

        if (walletId.HasValue)
        {
            query = query.Where(t => t.WalletId == walletId.Value);
        }

        if (month.HasValue)
        {
            query = query.Where(t => t.Date.Month == month.Value);
        }

        if (year.HasValue)
        {
            query = query.Where(t => t.Date.Year == year.Value);
        }

        var transactions = await query.ToListAsync();

        var categoryData = transactions
            .GroupBy(t => t.CategoryId)
            .Select(g => new CategoryReportItemDto
            {
                CategoryId = g.Key,
                CategoryName = g.First().Category.Name,
                TotalAmount = g.Sum(t => t.Amount)
            })
            .OrderByDescending(x => x.TotalAmount)
            .ToList();

        return categoryData;
    }
}
