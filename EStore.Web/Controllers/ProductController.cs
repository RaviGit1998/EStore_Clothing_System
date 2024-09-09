using EStore.Application.Service_Interfaces;
using EStore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        
        // GET: api/product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /*// POST: api/product
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            // Add validation and logic to create a product here
            // For demonstration, let's assume the product is created successfully
            // In real-world applications, you would implement creation logic and return the created entity

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        // PUT: api/product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            // Add validation and logic to update a product here
            // For demonstration, let's assume the update is successful
            // In real-world applications, you would implement the update logic

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            // Perform the update operation here
            return NoContent();
        }

        // DELETE: api/product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Add validation and logic to delete a product here
            // For demonstration, let's assume the deletion is successful
            // In real-world applications, you would implement the delete logic

            return NoContent();
        }*/
    }
}
