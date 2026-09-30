using Microsoft.AspNetCore.Mvc;
using _22_PhungDucAnh_Assignment01_FrontEnd.Services;

namespace _22_PhungDucAnh_Assignment01_FrontEnd.Controllers;

public class HomeController : Controller
{
    private readonly ApiService _api;

    public HomeController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index(string? search, short? categoryId)
    {
        var news = await _api.GetNewsArticlesAsync(onlyActive: true, search: search, categoryId: categoryId);
        var categories = await _api.GetCategoriesAsync();

        ViewBag.Categories = categories;
        ViewBag.CurrentSearch = search;
        ViewBag.CurrentCategory = categoryId;

        return View(news);
    }

    [HttpGet]
    public async Task<IActionResult> GetDetail(string id)
    {
        var article = await _api.GetNewsArticleByIdAsync(id);
        if (article == null) return NotFound();
        return Json(article);
    }
}