using Microsoft.Extensions.Caching.Memory;

namespace AdventureWorks.Service.Implements;

public class ReportService(IMemoryCache cache, IReportRepository reportRepository, IProductRepository productRepository) : IReportService
{

    public async Task<MonthlySalesReport> GetMonthlyReportAsync(int? year, int? month)
    {
        var now = DateTime.Now;
        int targetYear = year ?? now.Year;
        int targetMonth = month ?? now.Month;

        string cacheKey = $"monthly-report-{targetYear:D4}-{targetMonth:D2}";

        if (cache.TryGetValue(cacheKey, out var cachedObj) && cachedObj is MonthlySalesReport cachedResult)
        {
            return cachedResult;
        }

        var monthlyData = await reportRepository.GetSalesOrdersByMonthAsync(targetYear, targetMonth);
        var prevDate = new DateTime(targetYear, targetMonth, 1).AddMonths(-1);
        var lastMonthData = await reportRepository.GetSalesOrdersByMonthAsync(prevDate.Year, prevDate.Month);

        decimal totalAmount = monthlyData.Sum(x => x.SalesOrderDetails.Sum(d => d.LineTotal));
        int totalOrders = monthlyData.Count;

        var topProduct = monthlyData
            .SelectMany(h => h.SalesOrderDetails)
            .GroupBy(d => d.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQty = g.Sum(x => x.OrderQty) })
            .OrderByDescending(x => x.TotalQty)
            .FirstOrDefault();

        string? topProductName = null;
        if (topProduct != null)
        {
            var name = await productRepository.GetProductNameByIdAsync(topProduct.ProductId);
            if (!string.IsNullOrWhiteSpace(name))
                topProductName = name;
        }

        decimal lastMonthTotal = lastMonthData.Sum(x => x.SalesOrderDetails.Sum(d => d.LineTotal));
        decimal growthRate = lastMonthTotal == 0 ? 0 : ((totalAmount - lastMonthTotal) / lastMonthTotal) * 100;

        var result = new MonthlySalesReport
        {
            MonthYear = $"{targetMonth:00}/{targetYear}",
            TotalOrders = totalOrders,
            TotalAmount = totalAmount,
            TopSaleProduct = topProductName,
            GrowthRate = Math.Round(growthRate, 2)
        };

        cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
    }
}
