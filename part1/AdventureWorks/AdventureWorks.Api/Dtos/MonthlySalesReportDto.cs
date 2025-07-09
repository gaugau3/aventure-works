namespace AdventureWorks.Api.Dtos;

public class MonthlySalesReportDto
{
    public string MonthYear { get; set; } = default!;
    public int TotalOrders { get; set; }
    public decimal TotalAmount { get; set; }
    public string? TopSaleProduct { get; set; }
    public decimal GrowthRate { get; set; } // percentage
}
