
using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using EStore.Domain.EntityDtos;
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
            // Start query by filtering by category
            var query = _dbContext.Products
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.ProductVariants)
                .AsQueryable();

            // Apply price range filters if provided
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.ProductVariants.Any(v => v.PricePerUnit >= minPrice.Value));
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.ProductVariants.Any(v => v.PricePerUnit <= maxPrice.Value));
            }

            // Apply size filter only if size is provided
            if (!string.IsNullOrEmpty(size))
            {
                var sizes = size.Split(',');
                query = query.Where(p => p.ProductVariants.Any(v => sizes.Contains(v.Size)));
            }

            // Apply color filter only if color is provided
            if (!string.IsNullOrEmpty(color))
            {
                var colors = color.Split(',');
                query = query.Where(p => p.ProductVariants.Any(v => colors.Contains(v.Color)));
            }

            // Sorting logic
            query = sortOrder switch
            {
                "price_asc" => query.OrderBy(p => p.ProductVariants.Min(v => v.PricePerUnit)),
                "price_desc" => query.OrderByDescending(p => p.ProductVariants.Max(v => v.PricePerUnit)),
                _ => query,
            };

            return await query.ToListAsync();
        }



        public async Task<IEnumerable<ProductVariant>> GetProductVariants()
        {
            var productVariants = await _dbContext.ProductVariants.ToListAsync();

            return productVariants;
        }

        public async Task<ProductRespDto> GetProductByVariantIdAsync(int productVariantId)
        {
            var productVariant = await _dbContext.ProductVariants
                .Include(pv => pv.Product)
                    .ThenInclude(p => p.Category)
                .Include(pv => pv.Product)
                    .ThenInclude(p => p.SubCategory)
                .FirstOrDefaultAsync(pv => pv.ProductVariantId == productVariantId);
            if (productVariant == null)
            {
                return null;
            }
              var productResDto = new ProductRespDto
              {
                ProductId = productVariant.Product.ProductId,
                Name = productVariant.Product.Name,
                ShortDescription = productVariant.Product.ShortDescription,
                ImageData = Convert.ToBase64String(productVariant.Product.ImageData)
              };
          

            return productResDto;
        }

        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(
           int categoryId,
           decimal? minPrice,
           decimal? maxPrice)
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

            return await query.ToListAsync();
        }



    }
}
    









