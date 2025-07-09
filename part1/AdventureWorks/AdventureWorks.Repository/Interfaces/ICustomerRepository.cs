namespace AdventureWorks.Repository.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int customerId);
}
