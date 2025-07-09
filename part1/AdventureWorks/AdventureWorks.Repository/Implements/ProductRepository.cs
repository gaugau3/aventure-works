namespace AdventureWorks.Repository.Implements;

public class ProductRepository(AdventureWorksContext context) : IProductRepository
{
    private readonly AdventureWorksContext _context = context;
    public async Task<List<LowStockAlert>> GetLowStockProductsAsync()
    {
        var fakeNow = new DateTime(2014, 7, 1);
        var threeMonthsAgo = fakeNow.AddMonths(-3);

        // Step 1: Calculate the average sales volume in 3 months
        var avgSoldQuery = _context.SalesOrderDetails
            .Join(_context.SalesOrderHeaders,
                detail => detail.SalesOrderId,
                header => header.SalesOrderId,
                (detail, header) => new { detail.ProductId, header.OrderDate, detail.OrderQty })
            .Where(x => x.OrderDate >= threeMonthsAgo)
            .GroupBy(x => x.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                AvgSoldLast3Months = g.Sum(x => (decimal)x.OrderQty) / 3m
            });

        // Step 2: Join product + inventory + avgSold
        var query = from p in _context.Products
                    join pi in _context.ProductInventories on p.ProductId equals pi.ProductId
                    join avg in avgSoldQuery on p.ProductId equals avg.ProductId
                    where pi.Quantity < 2 * avg.AvgSoldLast3Months
                    select new LowStockAlert
                    {
                        ProductId = p.ProductId,
                        ProductName = p.Name,
                        StockQtty = pi.Quantity,
                        AvgSoldLast3Months = avg.AvgSoldLast3Months
                    };

        return await query.ToListAsync();
    }

    public async Task<string?> GetProductNameByIdAsync(int productId)
    {
        return await _context.Products
            .Where(p => p.ProductId == productId)
            .Select(p => p.Name)
            .FirstOrDefaultAsync();
    }
    public async Task<List<(int ProductId, int SpecialOfferId)>> GetExistingOfferProductPairsAsync(IEnumerable<(int ProductId, int SpecialOfferId)> pairs)
    {
        var productIds = pairs.Select(p => p.ProductId).ToList();
        var offerIds = pairs.Select(p => p.SpecialOfferId).ToList();

        return await _context.SpecialOfferProducts
            .Where(p => productIds.Contains(p.ProductId) && offerIds.Contains(p.SpecialOfferId))
            .Select(p => new ValueTuple<int, int>(p.ProductId, p.SpecialOfferId))
            .ToListAsync();
    }
}
