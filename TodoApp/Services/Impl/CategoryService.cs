using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Repositories;
using TodoApp.Repositories.Interfaces;
using TodoApp.ViewModels;

namespace TodoApp.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITaskRepository _taskRepository; // Thêm ITaskRepository

    public CategoryService(ICategoryRepository categoryRepository, ITaskRepository taskRepository)
    {
        _categoryRepository = categoryRepository;
        _taskRepository = taskRepository;
    }

    public ResultViewModel<List<CategoryViewModel>> GetAllCategories()
    {
        var categories = _categoryRepository.GetAllCategories();
        var result = categories.Select(c => new CategoryViewModel
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            Description = c.Description,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            IsActive = c.IsActive,
            IsDeleted = c.IsDeleted
        }).ToList();

        return ResultViewModel<List<CategoryViewModel>>.Success(result, "Lấy danh sách danh mục thành công");
    }

    public ResultViewModel<CategoryViewModel> GetCategoryById(int id)
    {
        var category = _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            return ResultViewModel<CategoryViewModel>.Failure("Không tìm thấy danh mục");
        }

        var categoryViewModel = new CategoryViewModel
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            IsActive = category.IsActive,
            IsDeleted = category.IsDeleted
        };

        return ResultViewModel<CategoryViewModel>.Success(categoryViewModel, "Lấy danh mục thành công");
    }

    public ResultViewModel<CategoryViewModel> CreateCategory(CategoryViewModel categoryViewModel)
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

        return ResultViewModel<CategoryViewModel>.Success(categoryViewModel, "Tạo mới danh mục thành công");
    }

    public ResultViewModel<CategoryViewModel> UpdateCategory(int id, CategoryViewModel categoryViewModel)
    {
        if (id != categoryViewModel.CategoryId)
        {
            return ResultViewModel<CategoryViewModel>.Failure("ID danh mục không hợp lệ");
        }

        var existing = _categoryRepository.GetCategoryById(id);
        if (existing == null)
        {
            return ResultViewModel<CategoryViewModel>.Failure("Không tìm thấy danh mục");
        }

        var category = new Category
        {
            CategoryId = categoryViewModel.CategoryId,
            Name = categoryViewModel.Name,
            Description = categoryViewModel.Description,
            CreatedAt = categoryViewModel.CreatedAt,
            UpdatedAt = DateTime.UtcNow,
            IsActive = categoryViewModel.IsActive,
            IsDeleted = categoryViewModel.IsDeleted
        };

        _categoryRepository.UpdateCategory(category);

        categoryViewModel.UpdatedAt = category.UpdatedAt;

        return ResultViewModel<CategoryViewModel>.Success(categoryViewModel, "Cập nhật danh mục thành công");
    }

    public ResultViewModel<bool> DeleteCategory(int id)
    {
        var category = _categoryRepository.GetCategoryById(id);
        if (category == null)
        {
            return ResultViewModel<bool>.Failure("Không tìm thấy danh mục để xóa");
        }
        
        var tasksByCategory = _taskRepository.GetAllTasks()
            .Where(t => t.CategoryId == id)
            .ToList();

        if (tasksByCategory.Count > 0)
        {
            return ResultViewModel<bool>.Failure("Không thể xóa danh mục vi đang có công việc liên quan");
        }

        // Cập nhật tất cả Task có CategoryId = id thành null
        var tasksToUpdate = _taskRepository.GetAllTasks()
            .Where(t => t.CategoryId == id)
            .ToList();
        
        
        // foreach (var task in tasksToUpdate)
        // {
        //     task.CategoryId = null;
        //     _taskRepository.UpdateTask(task);
        // }

        // Xóa Category
        var success = _categoryRepository.DeleteCategory(id);
        if (!success)
        {
            return ResultViewModel<bool>.Failure("Xóa danh mục thất bại");
        }

        return ResultViewModel<bool>.Success(true, "Xoá danh mục thành công");
    }
}