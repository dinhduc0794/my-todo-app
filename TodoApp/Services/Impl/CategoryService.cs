using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Repositories;
using TodoApp.ViewModels;

namespace TodoApp.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public List<CategoryViewModel> GetAllCategories()
    {
        var categories = _categoryRepository.GetAllCategories();
        return categories.Select(c => new CategoryViewModel
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            Description = c.Description,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            IsActive = c.IsActive,
            IsDeleted = c.IsDeleted
        }).ToList();
    }

    public CategoryViewModel GetCategoryById(int id)
    {
        var category = _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            return null;
        }

        return new CategoryViewModel
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            IsActive = category.IsActive,
            IsDeleted = category.IsDeleted
        };
    }

    public CategoryViewModel CreateCategory(CategoryViewModel categoryViewModel)
    {
        var category = new Category
        {
            Name = categoryViewModel.Name,
            Description = categoryViewModel.Description,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        };

        _categoryRepository.AddCategory(category);

        categoryViewModel.CategoryId = category.CategoryId;
        categoryViewModel.CreatedAt = category.CreatedAt;
        categoryViewModel.IsActive = category.IsActive;
        categoryViewModel.IsDeleted = category.IsDeleted;

        return categoryViewModel;
    }

    public bool UpdateCategory(int id, CategoryViewModel categoryViewModel)
    {
        if (id != categoryViewModel.CategoryId)
        {
            return false;
        }

        var category = new Category
        {
            CategoryId = categoryViewModel.CategoryId,
            Name = categoryViewModel.Name,
            Description = categoryViewModel.Description,
            CreatedAt = categoryViewModel.CreatedAt,
            IsActive = categoryViewModel.IsActive,
            IsDeleted = categoryViewModel.IsDeleted
        };

        return _categoryRepository.UpdateCategory(category);
    }

    public bool DeleteCategory(int id)
    {
        return _categoryRepository.DeleteCategory(id);
    }
}