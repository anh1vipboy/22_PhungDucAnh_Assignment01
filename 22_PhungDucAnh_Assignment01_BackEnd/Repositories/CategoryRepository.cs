using _22_PhungDucAnh_Assignment01_BackEnd.DataAccess;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;

namespace _22_PhungDucAnh_Assignment01_BackEnd.Repositories;

public interface ICategoryRepository
{
    IQueryable<Category> GetCategories();
    Task<Category?> GetCategoryByIdAsync(short id);
    Task<Category> AddCategoryAsync(Category category);
    Task<Category?> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(short id);
    Task<bool> HasNewsArticlesAsync(short categoryId);
}

public class CategoryRepository : ICategoryRepository
{
    public IQueryable<Category> GetCategories() => CategoryDAO.Instance.GetCategories();
    public Task<Category?> GetCategoryByIdAsync(short id) => CategoryDAO.Instance.GetCategoryByIdAsync(id);
    public Task<Category> AddCategoryAsync(Category category) => CategoryDAO.Instance.AddCategoryAsync(category);
    public Task<Category?> UpdateCategoryAsync(Category category) => CategoryDAO.Instance.UpdateCategoryAsync(category);
    public Task<bool> DeleteCategoryAsync(short id) => CategoryDAO.Instance.DeleteCategoryAsync(id);
    public Task<bool> HasNewsArticlesAsync(short categoryId) => CategoryDAO.Instance.HasNewsArticlesAsync(categoryId);
}