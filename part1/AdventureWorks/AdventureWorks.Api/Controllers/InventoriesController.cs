namespace AdventureWorks.Api.Controllers;

[Route("inventories")]
public class InventoriesController(IInventoryService inventoryService, IMapper mapper) : ApiControllerBase
{
    [HttpGet("low-stock-alerts")]
    public async Task<IActionResult> GetLowStockAlerts()
    {
        var domain = await inventoryService.GetLowStockAlertsAsync();
        var result = mapper.Map<List<LowStockAlertDto>>(domain);
        return Ok(result);
    }
}
