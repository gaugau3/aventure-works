using AdventureWorks.Domain;
using AdventureWorks.Repository.UnitOfWork;
using AdventureWorks.Service.Exceptions;

namespace AdventureWorks.Service.Implements;

public class OrderService(IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IOrderService
{
    public async Task CreateOrderAsync(Order order)
    {
        _ = await customerRepository.GetByIdAsync(order.CustomerId) ?? throw new ResourceNotFoundException($"Customer {order.CustomerId} does not found.");

        await ValidateOrderItemsAsync(order);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var header = new SalesOrderHeader
            {
                CustomerId = order.CustomerId,
                BillToAddressId = order.BillToAddressId,
                ShipToAddressId = order.ShipToAddressId,
                ShipMethodId = order.ShipMethodId,
                Comment = order.Comment,
                Status = (byte)OrderStatus.Pending,
                OrderDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(7),
                ModifiedDate = DateTime.UtcNow,
                SubTotal = 0,
                TaxAmt = 0,
                Freight = 0,
            };

            await orderRepository.AddAsync(header);
            await unitOfWork.SaveChangesAsync();

            var orderDetails = new List<SalesOrderDetail>();

            foreach (var item in order.Items)
            {
                orderDetails.Add(new SalesOrderDetail
                {
                    SalesOrderId = header.SalesOrderId,
                    ProductId = item.ProductId,
                    OrderQty = item.Quantity,
                    SpecialOfferId = item.SpecialOfferId, 
                    UnitPrice = 100m,   
                    UnitPriceDiscount = 0m, 
                    LineTotal = 100m * item.Quantity, 

                    ModifiedDate = DateTime.UtcNow,
                    Rowguid = Guid.NewGuid()
                });
            }
            await orderRepository.AddManyDetailsAsync(orderDetails);

            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw new InternalException();
        }
    }

    public async Task<List<SalesOrderHeader>> GetOrdersByCustomerAsync(int customerId)
    {
        _ = await customerRepository.GetByIdAsync(customerId) ?? throw new ResourceNotFoundException($"Customer {customerId} does not found.");
        var result = await orderRepository.GetOrdersByCustomerAsync(customerId);
        return result;
    }

    public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        var order = await orderRepository.GetByIdAsync(orderId) ?? throw new ResourceNotFoundException($"Order {orderId} does not exist.");
        order.Status = (byte)status;
        order.ModifiedDate = DateTime.UtcNow;
        await unitOfWork.SaveChangesAsync();
    }

    private async Task ValidateOrderItemsAsync(Order order)
    {
        var requestedPairs = order.Items
                                .Select(x => (x.ProductId, x.SpecialOfferId))
                                .Distinct()
                                .ToList();

        var existingPairs = await productRepository.GetExistingOfferProductPairsAsync(requestedPairs);

        var invalidPairs = requestedPairs
        .Except(existingPairs)
        .ToList();

        if (invalidPairs.Count != 0)
        {
            var formatted = string.Join(", ", invalidPairs.Select(p => $"(ProductId: {p.ProductId}, SpecialOfferId: {p.SpecialOfferId})"));
            throw new ParameterInvalidException($"Invalid product-offer combinations: {formatted}");
        }
    }
}
