using EStore.Application.Interfaces;
using EStore.Application.IRepositories;
using EStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            if(category == null)
            {
                throw new ArgumentNullException();
            }
           return await _categoryRepository.CreateCategoryAsync(category);    
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid product ID", nameof(id));
            }
           return await _categoryRepository.DeleteCategoryAsync(id);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid product ID", nameof(id));
            }
            return await _categoryRepository.GetCategoryAsync(id);
        }

        public async Task<Category> GetCategoryAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be null or empty", nameof(name));
            }

            return await _categoryRepository.GetCategoryAsync(name);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException();
            }
            return await _categoryRepository.UpdateCategoryAsync(category);
        } 
    }
}
