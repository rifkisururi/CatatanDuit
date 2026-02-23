using System.ComponentModel.DataAnnotations;
using CatatanDuit.Api.Models;

namespace CatatanDuit.Api.Dtos.Wallets;

public class CreateWalletRequestDto
{
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public WalletType Type { get; set; }

    [StringLength(100)]
    public string? AccountNumber { get; set; }

    [Required]
    public decimal InitialBalance { get; set; }
}
