namespace AdventureWorks.Repository.Interfaces;

public interface IReportRepository
{
    Task<List<SalesOrderHeader>> GetSalesOrdersByMonthAsync(int year, int month);
}
