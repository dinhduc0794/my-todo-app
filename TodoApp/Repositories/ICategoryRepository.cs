using TodoApp.Models;

namespace TodoApp.Repositories;

public interface ICategoryRepository
{
    List<Category> GetAllCategories();
    Category GetCategoryById(int id);
    void AddCategory(Category category);
    bool UpdateCategory(Category category);
    bool DeleteCategory(int id);
}