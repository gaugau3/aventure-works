using AdventureWorks.Domain;

namespace AdventureWorks.Api.AutoMappers;

public class OrderMapperProfile : ModelMapperProfileBase
{
    public OrderMapperProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
    }
}
