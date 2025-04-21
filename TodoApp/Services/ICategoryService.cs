using TodoApp.ViewModels;

namespace TodoApp.Services;

public interface ICategoryService
{
    ResultViewModel<List<CategoryViewModel>> GetAllCategories();
    ResultViewModel<CategoryViewModel> GetCategoryById(int id);
    ResultViewModel<CategoryViewModel> CreateCategory(CategoryViewModel categoryViewModel);
    ResultViewModel<CategoryViewModel> UpdateCategory(int id, CategoryViewModel categoryViewModel);
    ResultViewModel<bool> DeleteCategory(int id);
}