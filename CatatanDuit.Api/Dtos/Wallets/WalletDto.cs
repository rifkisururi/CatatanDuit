using CatatanDuit.Api.Models;

namespace CatatanDuit.Api.Dtos.Wallets;

public class WalletDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public WalletType Type { get; set; }
    public string? AccountNumber { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }
}
