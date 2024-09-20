using EStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.IRepositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductsByIdAsync(int id);
        Task<IEnumerable<Product>> SearchProductAsync(string keyword);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int productId);
        Task UpdateProductAsync(Product product);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetFilteredAndSortedProducts(int categoryId, decimal? minPrice, decimal? maxPrice, string size, string color, string sortOrder);
        Task<IEnumerable<ProductVariant>> GetProductVariants();
    }
}
