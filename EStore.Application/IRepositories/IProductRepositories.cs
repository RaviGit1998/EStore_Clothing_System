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
    }
}
