namespace Oblakoteka;

public interface IProductManagementService
{
    Task<IEnumerable<ProductViewModel>> GetProductsAsync(string filter);
    Task<ProductViewModel> GetProductAsync(Guid id);
    Task CreateProductAsync(ProductViewModel product);
    Task UpdateProductAsync(Guid id, ProductViewModel product);
    Task DeleteProductAsync(Guid id);
}