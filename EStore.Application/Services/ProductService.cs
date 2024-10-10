
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

        /*public async Task UpdateProductAsync(int productId, UpdateProductDto updateProductDto)
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
        }*/
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

            // Map updated product details
            _mapper.Map(updateProductDto, existingProduct);

            // Handle image upload if available
            if (updateProductDto.ImageFile != null && updateProductDto.ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updateProductDto.ImageFile.CopyToAsync(memoryStream);
                    existingProduct.ImageData = memoryStream.ToArray();
                }
            }

            // Update existing product variants or add new ones
            foreach (var variantDto in updateProductDto.ProductVariants)
            {
                var existingVariant = existingProduct.ProductVariants
                    .FirstOrDefault(v => v.ProductVariantId == variantDto.ProductVariantId);

                if (existingVariant != null)
                {
                    // Update existing variant
                    _mapper.Map(variantDto, existingVariant);
                }
                else
                {
                    // Add new variant
                    var newVariant = _mapper.Map<ProductVariant>(variantDto);
                    existingProduct.ProductVariants.Add(newVariant);
                }
            }

            // Remove deleted variants
            var variantIdsToUpdate = updateProductDto.ProductVariants.Select(v => v.ProductVariantId).ToList();
            foreach (var variant in existingProduct.ProductVariants.ToList())
            {
                if (!variantIdsToUpdate.Contains(variant.ProductVariantId))
                {
                    existingProduct.ProductVariants.Remove(variant);
                }
            }

            // Update product's modified date
            existingProduct.ModifiedDate = DateTime.UtcNow;

            // Save updated product and variants to the database
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
            size = string.IsNullOrWhiteSpace(size) ? null : size;
            color = string.IsNullOrWhiteSpace(color) ? null : color;

            try
            {
                var products = await _productRepository.GetFilteredAndSortedProducts(
                    categoryId, minPrice, maxPrice, size, color, sortOrder);

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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching products", ex);
            }
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

        public async Task<IEnumerable<ProductDto>> GetProductsByPriceRangeAsync(
         int categoryId,
         decimal? minPrice,
         decimal? maxPrice)
        {
            try
            {
                var products = await _productRepository.GetProductsByPriceRangeAsync(categoryId, minPrice, maxPrice);
                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

                // If you need to handle ImageData to ImageBase64 mapping
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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching products", ex);
            }
        }


        public async Task AddProductVariantAsync(ProductVariantDto productVariantDto)
        {
            // Map the DTO to entity
            var productVariant = _mapper.Map<ProductVariant>(productVariantDto);

            // Call the repository to add the variant
            await _productRepository.AddProductVariantAsync(productVariant);
        }

        public async Task UpdateProductVariantAsync(ProductVariantDto productVariantDto)
        {
            // Fetch existing product variant by ProductVariantId
            var existingProductVariant = await _productRepository.GetProductVariantsByProductIdAsync(productVariantDto.ProductId);

            if (existingProductVariant == null)
            {
                throw new Exception("Product variant not found");
            }

            // Map DTO to existing product variant entity
            _mapper.Map(productVariantDto, existingProductVariant);

            // Update product variant
            //await _productRepository.UpdateProductVariantAsync(existingProductVariant);
        }
        

            // Add Product with Variants
            public async Task AddProductWithVariantsAsync(CreateProductDto createProductDto)
            {
                // Map DTO to Product entity
              var product = _mapper.Map<Product>(createProductDto);
              if (createProductDto.ImageFile != null && createProductDto.ImageFile.Length > 0)
              {
                using (var memoryStream = new MemoryStream())
                {
                    await createProductDto.ImageFile.CopyToAsync(memoryStream);
                    product.ImageData = memoryStream.ToArray();
                }
              }
               await _productRepository.AddProductAsync(product);

              if (createProductDto.ProductVariants != null && createProductDto.ProductVariants.Any())
              {
                    foreach (var variantDto in createProductDto.ProductVariants)
                    {
                        var productVariant = _mapper.Map<ProductVariant>(variantDto);
                        productVariant.ProductId = product.ProductId; 
                       
                        await _productRepository.AddProductVariantAsync(productVariant);
                    }
              }
            }

            // Update Product with Variants
            public async Task UpdateProductWithVariantsAsync(int productId, UpdateProductDto updateProductDto)
            {
                // Fetch the existing product with variants
                var existingProduct = await _productRepository.GetProductsByIdAsync(productId);
                if (existingProduct == null)
                {
                    throw new KeyNotFoundException($"Product with ID {productId} not found.");
                }

                // Map updated product fields
                _mapper.Map(updateProductDto, existingProduct);

                // Handle ProductVariants
                foreach (var variantDto in updateProductDto.ProductVariants)
                {
                    var existingVariant = existingProduct.ProductVariants
                        .FirstOrDefault(v => v.ProductVariantId == variantDto.ProductVariantId);

                    if (existingVariant != null)
                    {
                        // Update existing variant
                        _mapper.Map(variantDto, existingVariant);
                    }
                    else
                    {
                        // Add new variant
                        var newVariant = _mapper.Map<ProductVariant>(variantDto);
                        newVariant.ProductId = productId;
                        existingProduct.ProductVariants.Add(newVariant);
                    }
                }

                // Remove variants that are not present in the updated list
                var variantsToRemove = existingProduct.ProductVariants
                    .Where(v => !updateProductDto.ProductVariants.Any(dto => dto.ProductVariantId == v.ProductVariantId))
                    .ToList();

                foreach (var variant in variantsToRemove)
                {
                    existingProduct.ProductVariants.Remove(variant);
                    await _productRepository.DeleteProductVariantAsync(variant.ProductVariantId);
                }

                // Save changes via repository
                await _productRepository.UpdateProductAsync(existingProduct);
            }
        

    }
}
