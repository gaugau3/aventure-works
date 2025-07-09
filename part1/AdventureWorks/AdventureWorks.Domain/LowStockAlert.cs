namespace AdventureWorks.Domain;

public class LowStockAlert
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = default!;
    public int StockQtty { get; set; }
    public decimal AvgSoldLast3Months { get; set; }
    public decimal ExpectedShortage => 2 * AvgSoldLast3Months - StockQtty;
    public bool IsShortage => StockQtty < 2 * AvgSoldLast3Months;
}
