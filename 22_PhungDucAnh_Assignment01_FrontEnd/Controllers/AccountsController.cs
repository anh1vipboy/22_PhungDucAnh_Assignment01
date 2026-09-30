using Microsoft.AspNetCore.Mvc;
using _22_PhungDucAnh_Assignment01_FrontEnd.Models;
using _22_PhungDucAnh_Assignment01_FrontEnd.Services;

namespace _22_PhungDucAnh_Assignment01_FrontEnd.Controllers;

public class AccountsController : Controller
{
    private readonly ApiService _api;

    public AccountsController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("RoleName") != "Admin")
        {
            return RedirectToAction("Login", "Auth");
        }

        var accounts = await _api.GetAccountsAsync();
        return View(accounts);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AccountViewModel model)
    {
        if (HttpContext.Session.GetString("RoleName") != "Admin") return RedirectToAction("Login", "Auth");

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please fill in all required fields.";
            return RedirectToAction("Index");
        }

        await _api.CreateAccountAsync(model);
        TempData["Success"] = "Account created successfully!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AccountViewModel model)
    {
        if (HttpContext.Session.GetString("RoleName") != "Admin") return RedirectToAction("Login", "Auth");

        await _api.UpdateAccountAsync(model);
        TempData["Success"] = "Account updated successfully!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(short id)
    {
        if (HttpContext.Session.GetString("RoleName") != "Admin") return RedirectToAction("Login", "Auth");

        var (success, error) = await _api.DeleteAccountAsync(id);
        if (success)
        {
            TempData["Success"] = "Account deleted successfully!";
        }
        else
        {
            TempData["Error"] = error;
        }
        return RedirectToAction("Index");
    }
}