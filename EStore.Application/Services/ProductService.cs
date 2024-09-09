using EStore.Application.Interfaces;
using EStore.Application.Service_Interfaces;
using EStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            
            return await _productRepository.GetAllProducts();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
           
            if (productId <= 0)
            {
                throw new ArgumentException("Invalid product ID", nameof(productId));
            }

            var product = await _productRepository.GetProductsByIdAsync(productId);

            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            return product;
        }
    }
}
