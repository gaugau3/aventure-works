using AdventureWorks.Api.Attributes;
using AdventureWorks.Domain;

namespace AdventureWorks.Api.Controllers;

[Route("orders")]
public class OrdersController (IOrderService orderService, IMapper mapper) : ApiControllerBase
{
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto dto)
    {
        var domain = mapper.Map<Order>(dto);
        await orderService.CreateOrderAsync(domain);
        return CreatedAtAction(nameof(GetOrdersByCustomer), new { customerId = domain.CustomerId }, domain);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetOrdersByCustomer(int customerId)
    {
        var orders = await orderService.GetOrdersByCustomerAsync(customerId);
        return Ok(orders);
    }

    [HttpPut("{orderId}/status")]
    public async Task<IActionResult> UpdateOrderStatus([FromRoute] int orderId, [FromBody] OrderStatus status)
    {
        await orderService.UpdateOrderStatusAsync(orderId, status);
        return NoContent();
    }
}
