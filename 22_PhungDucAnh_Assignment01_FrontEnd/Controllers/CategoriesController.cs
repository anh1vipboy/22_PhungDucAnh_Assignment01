using Microsoft.AspNetCore.Mvc;
using _22_PhungDucAnh_Assignment01_FrontEnd.Models;
using _22_PhungDucAnh_Assignment01_FrontEnd.Services;

namespace _22_PhungDucAnh_Assignment01_FrontEnd.Controllers;

public class CategoriesController : Controller
{
    private readonly ApiService _api;

    public CategoriesController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var role = HttpContext.Session.GetString("RoleName");
        if (role != "Staff" && role != "Admin") return RedirectToAction("Login", "Auth");

        var categories = await _api.GetCategoriesAsync();
        return View(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        var role = HttpContext.Session.GetString("RoleName");
        if (role != "Staff" && role != "Admin") return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid data provided.";
            return RedirectToAction("Index");
        }

        await _api.CreateCategoryAsync(model);
        TempData["Success"] = "Category created successfully!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryViewModel model)
    {
        var role = HttpContext.Session.GetString("RoleName");
        if (role != "Staff" && role != "Admin") return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid data provided.";
            return RedirectToAction("Index");
        }

        await _api.UpdateCategoryAsync(model);
        TempData["Success"] = "Category updated successfully!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(short id)
    {
        var role = HttpContext.Session.GetString("RoleName");
        if (role != "Staff" && role != "Admin") return RedirectToAction("Login", "Auth");

        var (success, error) = await _api.DeleteCategoryAsync(id);
        if (success)
        {
            TempData["Success"] = "Category deleted successfully!";
        }
        else
        {
            TempData["Error"] = error;
        }
        return RedirectToAction("Index");
    }
}