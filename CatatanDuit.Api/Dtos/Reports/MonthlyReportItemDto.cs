namespace CatatanDuit.Api.Dtos.Reports;

public class MonthlyReportItemDto
{
    public int Month { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal NetBalance { get; set; }
}
