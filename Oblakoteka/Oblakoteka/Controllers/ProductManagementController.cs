using Microsoft.AspNetCore.Mvc;

namespace Oblakoteka.Controllers
{
    public class ProductManagementController : Controller
    {
        private readonly IProductManagementService _productManagementService;

        public ProductManagementController(IProductManagementService productService)
        {
            _productManagementService = productService;
        }

        public async Task<IActionResult> Index(string filter)
        {
            try
            {
                var products = await _productManagementService.GetProductsAsync(filter);
                return View(products);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = "Произошла ошибка при загрузке списка продуктов. Пожалуйста, повторите попытку позже." });
            }
        }

        public IActionResult Add()
        {
            return PartialView("_ProductAddForm", new ProductViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productManagementService.CreateProductAsync(product);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = "Произошла ошибка при создании продукта. Пожалуйста, повторите попытку позже." });
            }
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var product = await _productManagementService.GetProductAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return PartialView("_ProductEditForm", product);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = "Произошла ошибка при загрузке данных продукта для редактирования. Пожалуйста, повторите попытку позже." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel product)
        {
            try
            {
                if (id != product.ID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _productManagementService.UpdateProductAsync(id, product);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = "Произошла ошибка при обновлении продукта. Пожалуйста, повторите попытку позже." });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _productManagementService.DeleteProductAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = "Произошла ошибка при удалении продукта. Пожалуйста, повторите попытку позже." });
            }
        }

        public IActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
