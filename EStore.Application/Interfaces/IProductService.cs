using EStore.Domain.Entities;
using EStore.Domain.EntityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> SearchProductAsync(string keyword);
        Task<int> AddProductAsync(CreateProductDto createProductDto);
        Task DeleteProductAsync(int productId);
        Task UpdateProductAsync(int productId, UpdateProductDto updateProductDto);


    }
}
