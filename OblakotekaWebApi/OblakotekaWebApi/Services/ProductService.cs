using Microsoft.EntityFrameworkCore;
using OblakotekaWebApi.Models;
using AutoMapper;

namespace OblakotekaWebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly TestDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(TestDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductViewModel>> GetProductsAsync(string? filter)
        {
            IQueryable<Product> query = _context.Product;

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.Name.Contains(filter));
            }

            var products = await query.ToListAsync();
            return _mapper.Map<IEnumerable<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel> GetProductAsync(Guid id)
        {
            var product = await _context.Product.FindAsync(id);
            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<ProductViewModel> CreateProductAsync(ProductViewModel productViewModel)
        {
            var product = _mapper.Map<Product>(productViewModel);
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task UpdateProductAsync(ProductViewModel productViewModel)
        {
            var product = _mapper.Map<Product>(productViewModel);
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
