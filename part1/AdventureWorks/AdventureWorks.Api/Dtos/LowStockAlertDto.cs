namespace AdventureWorks.Api.Dtos;

public class LowStockAlertDto
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int StockQtty { get; set; }
    public decimal AvgSoldLast3Months { get; set; }
    public decimal ExpectedShortage { get; set; }
}
