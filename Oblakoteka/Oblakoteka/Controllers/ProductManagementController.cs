using Microsoft.AspNetCore.Mvc;

namespace Oblakoteka.Controllers
{
    public class ProductManagementController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductManagementController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5087/");
        }

        public async Task<IActionResult> Index(string filter)
        {
            var url = "api/product";

            if (!string.IsNullOrEmpty(filter))
            {
                url += $"?filter={filter}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();
            return View(products);
        }

        public ActionResult Add()
        {
            var model = new ProductViewModel();

            return PartialView("_ProductAddForm", model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("api/product", product);
                response.EnsureSuccessStatusCode();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/product/{id}");
            response.EnsureSuccessStatusCode();

            var product = await response.Content.ReadFromJsonAsync<ProductViewModel>();
            return PartialView("_ProductEditForm", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel product)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"api/product/{id}", product);
                response.EnsureSuccessStatusCode();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/product/{id}");
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(Index));
        }
    }
}
