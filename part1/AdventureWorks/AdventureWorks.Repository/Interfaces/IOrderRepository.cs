namespace AdventureWorks.Repository.Interfaces;

public interface IOrderRepository
{
    Task<SalesOrderHeader?> GetByIdAsync(int orderId);
    Task<List<SalesOrderHeader>> GetOrdersByCustomerAsync(int customerId);
    Task<SalesOrderHeader> AddAsync(SalesOrderHeader order);
    Task AddManyDetailsAsync(List<SalesOrderDetail> orderDetails);
}
