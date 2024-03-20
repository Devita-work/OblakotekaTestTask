namespace OblakotekaWebApi;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> GetProductsAsync(string? filter);
    Task<ProductViewModel> GetProductAsync(Guid id);
    Task<ProductViewModel> CreateProductAsync(ProductViewModel productViewModel);
    Task UpdateProductAsync(ProductViewModel productViewModel);
    Task DeleteProductAsync(Guid id);
}