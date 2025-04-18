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
    
// SELECT c.*
// FROM categories c
// WHERE c.IsDeleted = 0
    public List<Category> GetAllCategories()
    {
        return _context.categories
            .Where(c => !c.IsDeleted)
            .ToList();
    }

    // SELECT c.*
// FROM categories c
// WHERE c.CategoryId = @id AND c.IsDeleted = 0
    public Category GetCategoryById(int id)
    {
        return _context.categories
            .FirstOrDefault(c => c.CategoryId == id && !c.IsDeleted);
    }

    // INSERT INTO categories (Name, Description, CreatedAt, UpdatedAt, IsActive, IsDeleted)
// VALUES (@Name, @Description, @CreatedAt, @UpdatedAt, @IsActive, @IsDeleted)
    public void AddCategory(Category category)
    {
        _context.categories.Add(category);
        _context.SaveChanges();
    }

    // UPDATE categories
// SET Name = @Name, Description = @Description, UpdatedAt = @UpdatedAt
// WHERE CategoryId = @CategoryId AND IsDeleted = 0
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

    // UPDATE categories
// SET IsDeleted = 1, UpdatedAt = @UpdatedAt
// WHERE CategoryId = @id AND IsDeleted = 0
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