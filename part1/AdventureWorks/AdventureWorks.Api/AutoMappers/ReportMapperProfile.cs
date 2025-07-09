using AdventureWorks.Domain;

namespace AdventureWorks.Api.AutoMappers;

public class ReportMapperProfile : ModelMapperProfileBase
{
    public ReportMapperProfile()
    {
        CreateMap<MonthlySalesReport, MonthlySalesReportDto>().ReverseMap();
    }
}
