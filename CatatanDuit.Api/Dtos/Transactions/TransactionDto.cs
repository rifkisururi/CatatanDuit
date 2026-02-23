using CatatanDuit.Api.Models;

namespace CatatanDuit.Api.Dtos.Transactions;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public string WalletName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public TransactionSource Source { get; set; }
}
