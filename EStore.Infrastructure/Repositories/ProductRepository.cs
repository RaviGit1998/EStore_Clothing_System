
using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using EStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EStore.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private EStoreDbContext _dbContext;
        public ProductRepository(EStoreDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _dbContext.Products
                .Include(pv => pv.ProductVariants)
                .ToListAsync();

        }

        public async Task<Product> GetProductsByIdAsync(int productId)
        {
            return await _dbContext.Products.Include(pv => pv.ProductVariants).
                FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<IEnumerable<Product>> SearchProductAsync(string keyword)
        {
            return await _dbContext.Products
                .Where(p => p.Name.Contains(keyword) || p.ShortDescription.Contains(keyword) || p.LongDesrciption.Contains(keyword) || p.Brand.Contains(keyword))
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .Include(p => p.ProductVariants)
                .ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _dbContext.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.ProductVariants)
                .Include(p => p.Category)
                .Include(p => p.SubCategory)
                .ToListAsync();
        }


        public async Task DeleteProductAsync(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetFilteredAndSortedProducts(
           int categoryId,
           decimal? minPrice,
           decimal? maxPrice,
           string size,
           string color,
           string sortOrder)
        {
            var query = _dbContext.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.ProductVariants)
                .AsQueryable();
          
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.ProductVariants.Any(v => v.PricePerUnit >= minPrice.Value));
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.ProductVariants.Any(v => v.PricePerUnit <= maxPrice.Value));
            }

            if (!string.IsNullOrEmpty(size))
            {
                query = query.Where(p => p.ProductVariants.Any(v => v.Size == size));
            }
        
            if (!string.IsNullOrEmpty(color))
            {
                query = query.Where(p => p.ProductVariants.Any(v => v.Color == color));
            }
           
            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(p => p.ProductVariants.Min(v => v.PricePerUnit)),
                "price_desc" => query.OrderByDescending(p => p.ProductVariants.Max(v => v.PricePerUnit)),
                _ => query,
            };

            return await query.ToListAsync();
        }

        
    }
}
    

