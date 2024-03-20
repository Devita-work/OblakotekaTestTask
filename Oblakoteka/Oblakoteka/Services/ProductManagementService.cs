using System.Text;
using Newtonsoft.Json;

namespace Oblakoteka;

public class ProductManagementService : IProductManagementService
{ 
    private readonly HttpClient _httpClient;

    public ProductManagementService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        var baseUrl = configuration.GetValue<string>("BaseUrl");
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<IEnumerable<ProductViewModel>> GetProductsAsync(string filter)
    {
        var url = "api/product";

        if (!string.IsNullOrEmpty(filter))
        {
            url += $"?filter={filter}";
        }

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();
    }

    public async Task<ProductViewModel> GetProductAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/product/{id}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ProductViewModel>();
    }

    public async Task CreateProductAsync(ProductViewModel product)
    {
        var jsonContent = JsonConvert.SerializeObject(product);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/product", httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateProductAsync(Guid id, ProductViewModel product)
    {
        var jsonContent = JsonConvert.SerializeObject(product);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"api/product/{id}", httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/product/{id}");
        response.EnsureSuccessStatusCode();
    }
}
