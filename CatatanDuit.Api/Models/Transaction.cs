namespace CatatanDuit.Api.Models;

public class Transaction : BaseEntity
{
    public Guid WalletId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public TransactionSource Source { get; set; }

    public Wallet Wallet { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public User User { get; set; } = null!;
}
