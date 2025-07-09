namespace AdventureWorks.Domain;

public class MonthlySalesReport
{
    public string MonthYear { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public decimal TotalAmount { get; set; }
    public string? TopSaleProduct { get; set; }
    public decimal GrowthRate { get; set; }
}
