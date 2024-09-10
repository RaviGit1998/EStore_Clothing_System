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
        Task<IEnumerable<ProductDto>> SearchAsync(string keyword);
        //Task AddAsync(CreateProductDto createProductDto);
        Task<int> AddAsync(CreateProductDto createProductDto);
        Task DeleteAsync(int productId);
        //Task<ProductDto> GetByIdAsync(int productId);
    }
}
