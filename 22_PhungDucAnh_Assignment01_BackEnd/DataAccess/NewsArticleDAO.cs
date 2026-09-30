using Microsoft.EntityFrameworkCore;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;

namespace _22_PhungDucAnh_Assignment01_BackEnd.DataAccess;

public class NewsArticleDAO
{
    private static NewsArticleDAO? _instance;
    private static readonly object _instanceLock = new();

    private NewsArticleDAO() { }

    public static NewsArticleDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                _instance ??= new NewsArticleDAO();
                return _instance;
            }
        }
    }

    public IQueryable<NewsArticle> GetNewsArticles()
    {
        var context = new FUNewsManagementContext();
        return context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .AsQueryable();
    }

    public async Task<NewsArticle?> GetNewsArticleByIdAsync(string id)
    {
        using var context = new FUNewsManagementContext();
        return await context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.NewsArticleId == id);
    }

    public async Task<NewsArticle> AddNewsArticleAsync(NewsArticle article, List<int> tagIds)
    {
        using var context = new FUNewsManagementContext();
        if (string.IsNullOrWhiteSpace(article.NewsArticleId))
        {
            int maxId = 0;
            var ids = await context.NewsArticles.Select(n => n.NewsArticleId).ToListAsync();
            foreach (var idStr in ids)
            {
                if (int.TryParse(idStr, out int num) && num > maxId)
                {
                    maxId = num;
                }
            }
            article.NewsArticleId = (maxId + 1).ToString();
        }

        article.CreatedDate ??= DateTime.UtcNow;

        if (tagIds != null && tagIds.Any())
        {
            var tags = await context.Tags.Where(t => tagIds.Contains(t.TagId)).ToListAsync();
            foreach (var tag in tags)
            {
                article.Tags.Add(tag);
            }
        }

        context.NewsArticles.Add(article);
        await context.SaveChangesAsync();
        return article;
    }

    public async Task<NewsArticle?> UpdateNewsArticleAsync(NewsArticle article, List<int> tagIds)
    {
        using var context = new FUNewsManagementContext();
        var existing = await context.NewsArticles
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.NewsArticleId == article.NewsArticleId);

        if (existing == null) return null;

        existing.NewsTitle = article.NewsTitle;
        existing.Headline = article.Headline;
        existing.NewsContent = article.NewsContent;
        existing.NewsSource = article.NewsSource;
        existing.CategoryId = article.CategoryId;
        existing.NewsStatus = article.NewsStatus;
        existing.UpdatedById = article.UpdatedById;
        existing.ModifiedDate = DateTime.UtcNow;

        // Update Tags
        existing.Tags.Clear();
        if (tagIds != null && tagIds.Any())
        {
            var tags = await context.Tags.Where(t => tagIds.Contains(t.TagId)).ToListAsync();
            foreach (var tag in tags)
            {
                existing.Tags.Add(tag);
            }
        }

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteNewsArticleAsync(string id)
    {
        using var context = new FUNewsManagementContext();
        var article = await context.NewsArticles
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.NewsArticleId == id);

        if (article == null) return false;

        article.Tags.Clear();
        context.NewsArticles.Remove(article);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<NewsArticle>> GetReportAsync(DateTime startDate, DateTime endDate)
    {
        using var context = new FUNewsManagementContext();
        return await context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
    }
}