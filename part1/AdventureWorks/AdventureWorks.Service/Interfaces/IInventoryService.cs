namespace AdventureWorks.Service.Interfaces;

public interface IInventoryService
{
    Task<List<LowStockAlert>> GetLowStockAlertsAsync();
}
