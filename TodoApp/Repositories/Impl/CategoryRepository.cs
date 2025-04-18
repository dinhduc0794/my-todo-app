using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly TodoAppDbContext _context;

    public CategoryRepository(TodoAppDbContext context)
    {
        _context = context;
    }

    public List<Category> GetAllCategories()
    {
        return _context.categories
            .Where(c => !c.IsDeleted)
            .ToList();
    }

    public Category GetCategoryById(int id)
    {
        return _context.categories
            .FirstOrDefault(c => c.CategoryId == id && !c.IsDeleted);
    }

    public void AddCategory(Category category)
    {
        _context.categories.Add(category);
        _context.SaveChanges();
    }

    public bool UpdateCategory(Category category)
    {
        var existingCategory = _context.categories.Find(category.CategoryId);
        if (existingCategory == null || existingCategory.IsDeleted)
        {
            return false;
        }

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;
        existingCategory.UpdatedAt = DateTime.UtcNow;

        try
        {
            _context.SaveChanges();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public bool DeleteCategory(int id)
    {
        var category = _context.categories.Find(id);
        if (category == null || category.IsDeleted)
        {
            return false;
        }

        category.IsDeleted = true;
        category.UpdatedAt = DateTime.UtcNow;
        _context.SaveChanges();
        return true;
    }
}