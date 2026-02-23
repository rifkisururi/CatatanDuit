using System.Collections.Generic;

namespace CatatanDuit.Api.Models;

public class Wallet : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public WalletType Type { get; set; }
    public string? AccountNumber { get; set; }
    public decimal InitialBalance { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = [];
}
