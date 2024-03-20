using Microsoft.AspNetCore.Mvc;

namespace OblakotekaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetProduct(string? filter)
        {
            try
            {
                var products = await _productService.GetProductsAsync(filter);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> GetProduct(Guid id)
        {
            try
            {
                var product = await _productService.GetProductAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductViewModel>> PostProduct(ProductViewModel productViewModel)
        {
            try
            {
                var createdProduct = await _productService.CreateProductAsync(productViewModel);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.ID }, createdProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, ProductViewModel productViewModel)
        {
            try
            {
                if (id != productViewModel.ID)
                {
                    return BadRequest();
                }

                await _productService.UpdateProductAsync(productViewModel);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
