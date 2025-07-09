namespace AdventureWorks.Repository.Interfaces;

public interface IProductRepository
{
    Task<List<LowStockAlert>> GetLowStockProductsAsync();
    Task<string?> GetProductNameByIdAsync(int productId);
    Task<List<(int ProductId, int SpecialOfferId)>> GetExistingOfferProductPairsAsync(IEnumerable<(int ProductId, int SpecialOfferId)> pairs);

}
