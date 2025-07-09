namespace AdventureWorks.Repository.Implements;

public class OrderRepository(AdventureWorksContext context) : IOrderRepository
{
    private readonly AdventureWorksContext _context = context;

    public async Task<SalesOrderHeader> AddAsync(SalesOrderHeader order)
    {
        var result = await _context.SalesOrderHeaders.AddAsync(order);
        return result.Entity;
    }

    public async Task AddManyDetailsAsync(List<SalesOrderDetail> orderDetails)
    {
        await _context.SalesOrderDetails.AddRangeAsync(orderDetails);
    }

    public async Task<SalesOrderHeader?> GetByIdAsync(int orderId)
    {
        return await _context.SalesOrderHeaders.FindAsync(orderId);
    }

    public async Task<List<SalesOrderHeader>> GetOrdersByCustomerAsync(int customerId)
    {
        return await _context.SalesOrderHeaders
                    .AsNoTracking()
                    .Where(o => o.CustomerId == customerId)
                    .ToListAsync();
    }
}
