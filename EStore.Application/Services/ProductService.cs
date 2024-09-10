
using AutoMapper;
using EStore.Application.Interfaces;
using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using EStore.Domain.EntityDtos;
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
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
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

        /*public async Task<ProductDto> GetByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductsByIdAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {productId} not found.");
            }
            return _mapper.Map<ProductDto>(product);
        }*/

        public async Task<IEnumerable<ProductDto>> SearchProductAsync(string keyword)
        {
            var products = await _productRepository.SearchProductAsync(keyword);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<int> AddProductAsync(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
            product.CreatedDate = DateTime.UtcNow;
            product.ModifiedDate = DateTime.UtcNow;        
            await _productRepository.AddProductAsync(product);
            return product.ProductId; 
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _productRepository.GetProductsByIdAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {productId} not found.");
            }
            await _productRepository.DeleteProductAsync(productId);
        }

    }
}
