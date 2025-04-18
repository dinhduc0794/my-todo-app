using TodoApp.ViewModels;

namespace TodoApp.Services;

public interface ICategoryService
{
    List<CategoryViewModel> GetAllCategories();
    CategoryViewModel GetCategoryById(int id);
    CategoryViewModel CreateCategory(CategoryViewModel categoryViewModel);
    bool UpdateCategory(int id, CategoryViewModel categoryViewModel);
    bool DeleteCategory(int id);
}