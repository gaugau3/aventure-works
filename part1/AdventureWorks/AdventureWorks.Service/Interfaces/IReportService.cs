namespace AdventureWorks.Service.Interfaces;

public interface IReportService
{
    Task<MonthlySalesReport> GetMonthlyReportAsync(int? year, int? month);
}
