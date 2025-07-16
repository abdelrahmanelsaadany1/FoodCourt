using Domain.Contracts;
using Domain.Entities;
using Services.Abstractions.ICategoryService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;

        public CategoryService(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task AddCategoryAsync(Category category)
        {
            // Example business rule: prevent duplicate name
            var existingCategories = await _categoryRepository.GetAllAsync();
            if (existingCategories.Any(c => c.Name == category.Name))
            {
                throw new Exception("Category name already exists.");
            }

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

       
    }
}

