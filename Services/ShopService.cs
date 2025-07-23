using bot.Database;
using bot.Models;
using Microsoft.EntityFrameworkCore;

namespace bot.Services
{
    public class ShopService
    {
        private readonly AppDbContext _shop;

        public ShopService(AppDbContext shop)
        {
            _shop = shop;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _shop.Set<Product>()
                .Where(p => p.IsAvailable)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<PagedResult<Product>> GetPagedProducts(int page = 1, int pageSize = 1)
        {
            var query = _shop.Set<Product>().Where(p => p.IsAvailable).OrderBy(p => p.Name);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await query.Skip(page * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Product>
            {
                Items = items,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };
        }

        public async Task<Product?> GetProductById(Guid id)
        {
            Product? produto = await _shop.Products.FirstOrDefaultAsync(p => p.Id.ToString() == id.ToString());
            return produto;
        }

        public async Task<List<Product>> GetProductsByCategory(string category)
        {
            return await _shop.Set<Product>()
                .Where(p => p.Category == category)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task AddProduct(Product product)
        {
            _shop.Set<Product>().Add(product);
            await _shop.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _shop.Set<Product>().Update(product);
            await _shop.SaveChangesAsync();
        }

        public async Task DeleteProduct(Guid id)
        {
            var product = await GetProductById(id);
            if (product != null)
            {
                _shop.Set<Product>().Remove(product);
                await _shop.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> SearchProducts(string searchTerm)
        {
            return await _shop.Set<Product>()
                .Where(p => p.IsAvailable && (p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}
