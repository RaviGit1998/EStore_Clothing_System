using EStore.Application.Interfaces;
using EStore.Application.Services;
using EStore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid category Id.");
            }
            try
            {
                var category = await _categoryService.GetCategoryAsync(id);
                return Ok(category);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<Category>> GetCategoryById([FromRoute] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Invalid category name");
            }
            try
            {
                var category = await _categoryService.GetCategoryAsync(name);
                return Ok(category);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid category Id");
            }

            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);

                if (!result)
                {
                    return NotFound("Category not found");
                }

            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category cannot be null");
            }
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return BadRequest("Category name cannot be null or empty");
            }
            try
            {
                var result = await _categoryService.CreateCategoryAsync(category);
                if (result == null)
                {
                  //in the service it returns null if category is already exists
                    return Conflict("A category with this name already exists");
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest("Invalid Input");
            }
          
        }
        [HttpPut]
    }
}
