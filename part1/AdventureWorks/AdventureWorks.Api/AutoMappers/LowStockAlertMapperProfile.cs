using AdventureWorks.Domain;

namespace AdventureWorks.Api.AutoMappers;

public class LowStockAlertMapperProfile : ModelMapperProfileBase
{
    public LowStockAlertMapperProfile()
    {
        CreateMap<LowStockAlert, LowStockAlertDto>().ReverseMap();
    }
}
