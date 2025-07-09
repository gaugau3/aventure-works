namespace AdventureWorks.Api.Dtos;

public class OrderItemDto
{
    public int ProductId { get; set; }
    public short Quantity { get; set; }
    public int SpecialOfferId { get; set; }
}