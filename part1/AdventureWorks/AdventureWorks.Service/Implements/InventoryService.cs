namespace AdventureWorks.Service.Implements;

public class InventoryService(IProductRepository productRepository) : IInventoryService
{
    public async Task<List<LowStockAlert>> GetLowStockAlertsAsync()
    {
        return await productRepository.GetLowStockProductsAsync() ?? [];
    }
}
