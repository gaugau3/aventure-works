namespace AdventureWorks.Repository.Implements;

public class CustomerRepository(AdventureWorksContext context) : ICustomerRepository
{
    private readonly AdventureWorksContext _context = context;
    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);
    }
}
