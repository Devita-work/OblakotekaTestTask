using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Oblakoteka.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7126/");
        }

        [HttpGet]
        public async Task<ActionResult> GetProduct(string? filter)
        {
            var url = $"api/product";

            if (!string.IsNullOrEmpty(filter))
            {
                url += $"?filter={filter}";
            }

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductViewModel>>(content);

            return Ok(products);
        }

        // GET: api/WebApp/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetProduct(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/product/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var product = await response.Content.ReadFromJsonAsync<ProductViewModel>();
            return product;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductViewModel product)
        {
            var jsonContent = JsonConvert.SerializeObject(product);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/product", httpContent);

            if (!response.IsSuccessStatusCode)
                return Problem("Failed to create product", null, (int)response.StatusCode);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, ProductViewModel product)
        {
            var jsonContent = JsonConvert.SerializeObject(product);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/product/{id}", httpContent);

            if (!response.IsSuccessStatusCode)
                return Problem("Failed to update product", null, (int)response.StatusCode);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/product/{id}");

            if (!response.IsSuccessStatusCode)
                return Problem("Failed to delete product", null, (int)response.StatusCode);

            return NoContent();
        }
    }
}
