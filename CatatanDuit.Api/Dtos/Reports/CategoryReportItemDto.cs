namespace CatatanDuit.Api.Dtos.Reports;

public class CategoryReportItemDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}
