namespace CatatanDuit.Api.Dtos.Reports;

public class WalletSummaryDto
{
    public Guid WalletId { get; set; }
    public string WalletName { get; set; } = string.Empty;
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal CurrentBalance { get; set; }
}
