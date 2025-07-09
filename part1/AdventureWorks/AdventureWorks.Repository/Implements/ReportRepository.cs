namespace AdventureWorks.Repository.Implements;

public class ReportRepository(AdventureWorksContext context) : IReportRepository
{
    private readonly AdventureWorksContext _context = context;
    public async Task<List<SalesOrderHeader>> GetSalesOrdersByMonthAsync(int year, int month)
    {
        return await _context.SalesOrderHeaders
            .Include(h => h.SalesOrderDetails)
            .Where(h =>
            (h.OrderDate.Year == year) &&
            (h.OrderDate.Month == month))
            .ToListAsync();
    }

}
