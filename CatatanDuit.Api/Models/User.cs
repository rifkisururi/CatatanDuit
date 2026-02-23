using System.Collections.Generic;

namespace CatatanDuit.Api.Models;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string? Name { get; set; }

    public ICollection<Wallet> Wallets { get; set; } = [];
    public ICollection<Category> Categories { get; set; } = [];
    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<ApiKey> ApiKeys { get; set; } = [];
}
