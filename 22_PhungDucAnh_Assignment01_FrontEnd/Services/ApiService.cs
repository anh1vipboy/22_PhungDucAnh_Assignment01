using System.Text;
using System.Text.Json;
using _22_PhungDucAnh_Assignment01_FrontEnd.Models;

namespace _22_PhungDucAnh_Assignment01_FrontEnd.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOpt = new() { PropertyNameCaseInsensitive = true };

    public ApiService(HttpClient http)
    {
        _http = http;
        if (_http.BaseAddress == null)
        {
            _http.BaseAddress = new Uri("http://localhost:5100/");
        }
    }

    // Helper OData deserializer
    private async Task<List<T>> GetODataListAsync<T>(string endpoint)
    {
        try
        {
            var res = await _http.GetAsync(endpoint);
            if (!res.IsSuccessStatusCode) return new List<T>();
            var json = await res.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("value", out var valElement))
            {
                return JsonSerializer.Deserialize<List<T>>(valElement.GetRawText(), _jsonOpt) ?? new List<T>();
            }
            return new List<T>();
        }
        catch
        {
            return new List<T>();
        }
    }

    // Auth
    public async Task<LoginResponseModel?> LoginAsync(string email, string password)
    {
        var content = new StringContent(JsonSerializer.Serialize(new { email, password }), Encoding.UTF8, "application/json");
        var res = await _http.PostAsync("api/auth/login", content);
        if (!res.IsSuccessStatusCode) return null;
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<LoginResponseModel>(json, _jsonOpt);
    }

    // News Articles
    public async Task<List<NewsArticleViewModel>> GetNewsArticlesAsync(bool onlyActive = false, string? search = null, short? categoryId = null)
    {
        var query = "odata/NewsArticles?$expand=Category,CreatedBy,Tags&$orderby=CreatedDate desc";
        var filters = new List<string>();
        if (onlyActive) filters.Add("NewsStatus eq true");
        if (categoryId.HasValue && categoryId.Value > 0) filters.Add($"CategoryId eq {categoryId.Value}");
        if (!string.IsNullOrWhiteSpace(search))
        {
            filters.Add($"(contains(tolower(NewsTitle), '{search.ToLower()}') or contains(tolower(Headline), '{search.ToLower()}'))");
        }

        if (filters.Any())
        {
            query += "&$filter=" + string.Join(" and ", filters);
        }

        return await GetODataListAsync<NewsArticleViewModel>(query);
    }

    public async Task<NewsArticleViewModel?> GetNewsArticleByIdAsync(string id)
    {
        var list = await GetODataListAsync<NewsArticleViewModel>($"odata/NewsArticles?$filter=NewsArticleId eq '{id}'&$expand=Category,CreatedBy,Tags");
        return list.FirstOrDefault();
    }

    public async Task<bool> CreateNewsArticleAsync(NewsArticleViewModel model, List<int> tagIds, short createdById)
    {
        var payload = new
        {
            model.NewsArticleId,
            model.NewsTitle,
            model.Headline,
            model.NewsContent,
            model.NewsSource,
            model.CategoryId,
            model.NewsStatus,
            CreatedById = createdById,
            TagIds = tagIds
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var res = await _http.PostAsync("odata/NewsArticles", content);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateNewsArticleAsync(NewsArticleViewModel model, List<int> tagIds, short updatedById)
    {
        var payload = new
        {
            model.NewsArticleId,
            model.NewsTitle,
            model.Headline,
            model.NewsContent,
            model.NewsSource,
            model.CategoryId,
            model.NewsStatus,
            UpdatedById = updatedById,
            TagIds = tagIds
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var res = await _http.PutAsync($"odata/NewsArticles('{model.NewsArticleId}')", content);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteNewsArticleAsync(string id)
    {
        var res = await _http.DeleteAsync($"odata/NewsArticles('{id}')");
        return res.IsSuccessStatusCode;
    }

    // Categories
    public async Task<List<CategoryViewModel>> GetCategoriesAsync()
    {
        return await GetODataListAsync<CategoryViewModel>("odata/Categories?$orderby=CategoryName");
    }

    public async Task<bool> CreateCategoryAsync(CategoryViewModel model)
    {
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        var res = await _http.PostAsync("odata/Categories", content);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCategoryAsync(CategoryViewModel model)
    {
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        var res = await _http.PutAsync($"odata/Categories({model.CategoryId})", content);
        return res.IsSuccessStatusCode;
    }

    public async Task<(bool success, string error)> DeleteCategoryAsync(short id)
    {
        var res = await _http.DeleteAsync($"odata/Categories({id})");
        if (res.IsSuccessStatusCode) return (true, string.Empty);
        var msg = await res.Content.ReadAsStringAsync();
        return (false, string.IsNullOrWhiteSpace(msg) ? "Cannot delete Category because it already has News Articles." : msg);
    }

    // Accounts
    public async Task<List<AccountViewModel>> GetAccountsAsync()
    {
        return await GetODataListAsync<AccountViewModel>("odata/SystemAccounts?$orderby=AccountName");
    }

    public async Task<AccountViewModel?> GetAccountByIdAsync(short id)
    {
        var list = await GetODataListAsync<AccountViewModel>($"odata/SystemAccounts?$filter=AccountId eq {id}");
        return list.FirstOrDefault();
    }

    public async Task<bool> CreateAccountAsync(AccountViewModel model)
    {
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        var res = await _http.PostAsync("odata/SystemAccounts", content);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAccountAsync(AccountViewModel model)
    {
        var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
        var res = await _http.PutAsync($"odata/SystemAccounts({model.AccountId})", content);
        return res.IsSuccessStatusCode;
    }

    public async Task<(bool success, string error)> DeleteAccountAsync(short id)
    {
        var res = await _http.DeleteAsync($"odata/SystemAccounts({id})");
        if (res.IsSuccessStatusCode) return (true, string.Empty);
        var msg = await res.Content.ReadAsStringAsync();
        return (false, string.IsNullOrWhiteSpace(msg) ? "Cannot delete Account because this account has already created News Articles." : msg);
    }

    // Tags
    public async Task<List<TagViewModel>> GetTagsAsync()
    {
        return await GetODataListAsync<TagViewModel>("odata/Tags?$orderby=TagName");
    }

    // Reports
    public async Task<ReportViewModel> GetReportAsync(DateTime? start, DateTime? end)
    {
        var query = "api/reports/statistics";
        var parts = new List<string>();
        if (start.HasValue) parts.Add($"startDate={start.Value:yyyy-MM-dd}");
        if (end.HasValue) parts.Add($"endDate={end.Value:yyyy-MM-dd}");
        if (parts.Any()) query += "?" + string.Join("&", parts);

        var res = await _http.GetAsync(query);
        if (!res.IsSuccessStatusCode) return new ReportViewModel();
        var json = await res.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ReportViewModel>(json, _jsonOpt) ?? new ReportViewModel();
    }
}