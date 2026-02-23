using CatatanDuit.Api.Dtos.Reports;

namespace CatatanDuit.Api.Services.Interfaces;

public interface IReportService
{
    Task<IEnumerable<WalletSummaryDto>> GetSummaryAsync(Guid userId);
    Task<IEnumerable<MonthlyReportItemDto>> GetMonthlyReportAsync(Guid userId, int year);
    Task<IEnumerable<CategoryReportItemDto>> GetByCategoryAsync(Guid userId, Guid? walletId, int? month, int? year);
}
