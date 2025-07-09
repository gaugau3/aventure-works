
namespace AdventureWorks.Service.Interfaces;

public interface IOrderService
{
    Task<List<SalesOrderHeader>> GetOrdersByCustomerAsync(int customerId);
    Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
    Task CreateOrderAsync(Order order);
}
