using AdventureWorks.Api.Attributes;
using AdventureWorks.Service.Exceptions;

namespace AdventureWorks.Api.Controllers;

[Route("reports")]
public class ReportsController(IReportService reportService, IMapper mapper) : ApiControllerBase
{
    [HttpGet("monthly-sales")]
    [ValidateModel]
    public async Task<IActionResult> GetMonthlySalesReport([FromQuery] MonthlyReportDto monthlyReportDto)
    {
        var domain = await reportService.GetMonthlyReportAsync(monthlyReportDto.Year, monthlyReportDto.Month) ?? throw new ResourceNotFoundException();
        var result = mapper.Map<MonthlySalesReportDto>(domain);
        return Ok(result);
    }
}
