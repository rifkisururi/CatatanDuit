using System.ComponentModel.DataAnnotations;
using CatatanDuit.Api.Models;

namespace CatatanDuit.Api.Dtos.Transactions;

public class CreateTransactionRequestDto
{
    [Required]
    public Guid WalletId { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public TransactionType Type { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    public DateTime Date { get; set; }
}
