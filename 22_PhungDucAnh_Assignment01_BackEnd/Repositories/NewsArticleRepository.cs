using _22_PhungDucAnh_Assignment01_BackEnd.DataAccess;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;

namespace _22_PhungDucAnh_Assignment01_BackEnd.Repositories;

public interface INewsArticleRepository
{
    IQueryable<NewsArticle> GetNewsArticles();
    Task<NewsArticle?> GetNewsArticleByIdAsync(string id);
    Task<NewsArticle> AddNewsArticleAsync(NewsArticle article, List<int> tagIds);
    Task<NewsArticle?> UpdateNewsArticleAsync(NewsArticle article, List<int> tagIds);
    Task<bool> DeleteNewsArticleAsync(string id);
    Task<List<NewsArticle>> GetReportAsync(DateTime startDate, DateTime endDate);
}

public class NewsArticleRepository : INewsArticleRepository
{
    public IQueryable<NewsArticle> GetNewsArticles() => NewsArticleDAO.Instance.GetNewsArticles();
    public Task<NewsArticle?> GetNewsArticleByIdAsync(string id) => NewsArticleDAO.Instance.GetNewsArticleByIdAsync(id);
    public Task<NewsArticle> AddNewsArticleAsync(NewsArticle article, List<int> tagIds) => NewsArticleDAO.Instance.AddNewsArticleAsync(article, tagIds);
    public Task<NewsArticle?> UpdateNewsArticleAsync(NewsArticle article, List<int> tagIds) => NewsArticleDAO.Instance.UpdateNewsArticleAsync(article, tagIds);
    public Task<bool> DeleteNewsArticleAsync(string id) => NewsArticleDAO.Instance.DeleteNewsArticleAsync(id);
    public Task<List<NewsArticle>> GetReportAsync(DateTime startDate, DateTime endDate) => NewsArticleDAO.Instance.GetReportAsync(startDate, endDate);
}