namespace AdventureWorks.Api.Dtos;

public class OrderDto
{
    public int CustomerId { get; set; }
    public int BillToAddressId { get; set; }
    public int ShipToAddressId { get; set; }
    public int ShipMethodId { get; set; }
    public string? Comment { get; set; }
    public List<OrderItemDto> Items { get; set; } = [];
}
