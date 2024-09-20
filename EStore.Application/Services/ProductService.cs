
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

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProducts();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            foreach (var productDto in productDtos)
            {
                var product = products.FirstOrDefault(p => p.ProductId == productDto.ProductId);
                if (product?.ImageData != null)
                {
                    productDto.ImageBase64 = Convert.ToBase64String(product.ImageData);
                }
            }

            return productDtos;
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
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

            var productDto = _mapper.Map<ProductDto>(product);

            // Convert image data to base64 string
            if (product.ImageData != null && product.ImageData.Length > 0)
            {
                productDto.ImageBase64 = Convert.ToBase64String(product.ImageData);
            }

            return productDto;

        }
        public async Task<IEnumerable<ProductDto>> SearchProductAsync(string keyword)
        {
            var products = await _productRepository.SearchProductAsync(keyword);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            foreach (var productDto in productDtos)
            {
                var product = products.FirstOrDefault(p => p.ProductId == productDto.ProductId);
                if (product?.ImageData != null)
                {
                    productDto.ImageBase64 = Convert.ToBase64String(product.ImageData);
                }
            }

            return productDtos;
        }

        public async Task<int> AddProductAsync(CreateProductDto createProductDto)
        {
            var product = _mapper.Map<Product>(createProductDto);
            if (createProductDto.ImageFile != null && createProductDto.ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await createProductDto.ImageFile.CopyToAsync(memoryStream);
                    product.ImageData = memoryStream.ToArray();
                }
            }         
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

        public async Task UpdateProductAsync(int productId, UpdateProductDto updateProductDto)
        {
            if (updateProductDto == null)
            {
                throw new ArgumentNullException(nameof(updateProductDto));
            }

            var existingProduct = await _productRepository.GetProductsByIdAsync(productId);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            _mapper.Map(updateProductDto, existingProduct);
            if (updateProductDto.ImageFile != null && updateProductDto.ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updateProductDto.ImageFile.CopyToAsync(memoryStream);
                    existingProduct.ImageData = memoryStream.ToArray();
                }
            }
            existingProduct.ModifiedDate = DateTime.UtcNow;

            await _productRepository.UpdateProductAsync(existingProduct);
        }
        
        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            foreach (var productDto in productDtos)
            {
                var product = products.FirstOrDefault(p => p.ProductId == productDto.ProductId);
                if (product?.ImageData != null)
                {
                    productDto.ImageBase64 = Convert.ToBase64String(product.ImageData);
                }
            }

            return productDtos;
        }

        public async Task<IEnumerable<ProductDto>> GetFilteredAndSortedProductsAsync(
            int categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            string size,
            string color,
            string sortOrder)
        {
            var products = await _productRepository.GetFilteredAndSortedProducts(categoryId, minPrice, maxPrice, size, color, sortOrder);
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            foreach (var productDto in productDtos)
            {
                var product = products.FirstOrDefault(p => p.ProductId == productDto.ProductId);
                if (product?.ImageData != null)
                {
                    productDto.ImageBase64 = Convert.ToBase64String(product.ImageData);
                }
            }
            return productDtos;
        }
        public async Task<IEnumerable<ProductVariant>> GetProductVariants()
        {
            var productVariants = await _productRepository.GetProductVariants();
            return productVariants;

        }

        public async Task<ProductRespDto> GetProductByVariantIdAsync(int productVariantId)
        {
            var productResDto= await _productRepository.GetProductByVariantIdAsync(productVariantId);           
            return productResDto;
        }
    }
}
