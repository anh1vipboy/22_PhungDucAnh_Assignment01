using Microsoft.AspNetCore.Mvc;
using _22_PhungDucAnh_Assignment01_FrontEnd.Models;
using _22_PhungDucAnh_Assignment01_FrontEnd.Services;

namespace _22_PhungDucAnh_Assignment01_FrontEnd.Controllers;

public class NewsArticlesController : Controller
{
    private readonly ApiService _api;

    public NewsArticlesController(ApiService api)
    {
        _api = api;
    }

    private bool CheckStaffOrAdmin()
    {
        var role = HttpContext.Session.GetString("RoleName");
        return role == "Staff" || role == "Admin";
    }

    public async Task<IActionResult> Index(string? search, short? categoryId)
    {
        if (!CheckStaffOrAdmin()) return RedirectToAction("Login", "Auth");

        var news = await _api.GetNewsArticlesAsync(onlyActive: false, search: search, categoryId: categoryId);
        ViewBag.Categories = await _api.GetCategoriesAsync();
        ViewBag.Tags = await _api.GetTagsAsync();
        ViewBag.CurrentSearch = search;
        ViewBag.CurrentCategory = categoryId;

        return View(news);
    }

    public async Task<IActionResult> History()
    {
        if (HttpContext.Session.GetString("RoleName") != "Staff") return RedirectToAction("Login", "Auth");

        short staffId = (short)(HttpContext.Session.GetInt32("AccountId") ?? 0);
        var allNews = await _api.GetNewsArticlesAsync(onlyActive: false);
        var myNews = allNews.Where(n => n.CreatedById == staffId).ToList();

        return View(myNews);
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewsArticleViewModel model, List<int> selectedTags)
    {
        if (!CheckStaffOrAdmin()) return RedirectToAction("Login", "Auth");

        short currentUserId = (short)(HttpContext.Session.GetInt32("AccountId") ?? 1);
        await _api.CreateNewsArticleAsync(model, selectedTags, currentUserId);
        TempData["Success"] = "News Article created successfully!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(NewsArticleViewModel model, List<int> selectedTags)
    {
        if (!CheckStaffOrAdmin()) return RedirectToAction("Login", "Auth");

        short currentUserId = (short)(HttpContext.Session.GetInt32("AccountId") ?? 1);
        await _api.UpdateNewsArticleAsync(model, selectedTags, currentUserId);
        TempData["Success"] = "News Article updated successfully!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        if (!CheckStaffOrAdmin()) return RedirectToAction("Login", "Auth");

        await _api.DeleteNewsArticleAsync(id);
        TempData["Success"] = "News Article deleted successfully!";
        return RedirectToAction("Index");
    }
}