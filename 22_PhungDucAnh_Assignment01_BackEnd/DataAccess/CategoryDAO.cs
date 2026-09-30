using Microsoft.EntityFrameworkCore;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;

namespace _22_PhungDucAnh_Assignment01_BackEnd.DataAccess;

public class CategoryDAO
{
    private static CategoryDAO? _instance;
    private static readonly object _instanceLock = new();

    private CategoryDAO() { }

    public static CategoryDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                _instance ??= new CategoryDAO();
                return _instance;
            }
        }
    }

    public IQueryable<Category> GetCategories()
    {
        var context = new FUNewsManagementContext();
        return context.Categories.AsQueryable();
    }

    public async Task<Category?> GetCategoryByIdAsync(short id)
    {
        using var context = new FUNewsManagementContext();
        return await context.Categories
            .Include(c => c.ParentCategory)
            .FirstOrDefaultAsync(c => c.CategoryId == id);
    }

    public async Task<Category> AddCategoryAsync(Category category)
    {
        using var context = new FUNewsManagementContext();
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> UpdateCategoryAsync(Category category)
    {
        using var context = new FUNewsManagementContext();
        var existing = await context.Categories.FindAsync(category.CategoryId);
        if (existing == null) return null;

        existing.CategoryName = category.CategoryName;
        existing.CategoryDesciption = category.CategoryDesciption;
        existing.ParentCategoryId = category.ParentCategoryId;
        existing.IsActive = category.IsActive;

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteCategoryAsync(short id)
    {
        using var context = new FUNewsManagementContext();
        var category = await context.Categories.FindAsync(id);
        if (category == null) return false;

        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasNewsArticlesAsync(short categoryId)
    {
        using var context = new FUNewsManagementContext();
        return await context.NewsArticles.AnyAsync(n => n.CategoryId == categoryId);
    }
}