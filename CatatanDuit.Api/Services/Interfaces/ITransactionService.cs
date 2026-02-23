using CatatanDuit.Api.Common;
using CatatanDuit.Api.Dtos.Transactions;
using CatatanDuit.Api.Models;

namespace CatatanDuit.Api.Services.Interfaces;

public interface ITransactionService
{
    Task<PagedResult<TransactionDto>> GetTransactionsAsync(
        Guid userId,
        Guid? walletId,
        Guid? categoryId,
        TransactionType? type,
        DateTime? dateFrom,
        DateTime? dateTo,
        int page,
        int pageSize);

    Task<TransactionDto?> GetTransactionByIdAsync(Guid userId, Guid transactionId);
    Task<TransactionDto> CreateTransactionAsync(Guid userId, CreateTransactionRequestDto dto);
    Task<TransactionDto?> UpdateTransactionAsync(Guid userId, Guid transactionId, UpdateTransactionRequestDto dto);
    Task<bool> DeleteTransactionAsync(Guid userId, Guid transactionId);
}
