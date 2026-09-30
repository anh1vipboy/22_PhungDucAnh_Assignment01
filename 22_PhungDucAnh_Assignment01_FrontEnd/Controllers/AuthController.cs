using Microsoft.AspNetCore.Mvc;
using _22_PhungDucAnh_Assignment01_FrontEnd.Models;
using _22_PhungDucAnh_Assignment01_FrontEnd.Services;

namespace _22_PhungDucAnh_Assignment01_FrontEnd.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _api;

    public AuthController(ApiService api)
    {
        _api = api;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("RoleName") != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _api.LoginAsync(model.Email, model.Password);
        if (user == null)
        {
            ViewBag.Error = "Invalid email or password.";
            return View(model);
        }

        // LÆ°u thÃ´ng tin Ä‘Äƒng nháº­p vÃ o Session
        HttpContext.Session.SetInt32("AccountId", user.AccountId);
        HttpContext.Session.SetString("AccountName", user.AccountName);
        HttpContext.Session.SetString("AccountEmail", user.AccountEmail);
        HttpContext.Session.SetInt32("AccountRole", user.AccountRole);
        HttpContext.Session.SetString("RoleName", user.RoleName);

        if (user.RoleName == "Admin")
        {
            return RedirectToAction("Index", "Accounts");
        }
        else
        {
            return RedirectToAction("Index", "NewsArticles");
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}