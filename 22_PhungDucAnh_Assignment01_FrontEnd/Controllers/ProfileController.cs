using Microsoft.AspNetCore.Mvc;
using _22_PhungDucAnh_Assignment01_FrontEnd.Models;
using _22_PhungDucAnh_Assignment01_FrontEnd.Services;

namespace _22_PhungDucAnh_Assignment01_FrontEnd.Controllers;

public class ProfileController : Controller
{
    private readonly ApiService _api;

    public ProfileController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var role = HttpContext.Session.GetString("RoleName");
        if (role != "Staff" && role != "Lecturer") return RedirectToAction("Login", "Auth");

        short accountId = (short)(HttpContext.Session.GetInt32("AccountId") ?? 0);
        var account = await _api.GetAccountByIdAsync(accountId);
        if (account == null) return NotFound();

        return View(account);
    }

    [HttpPost]
    public async Task<IActionResult> Update(AccountViewModel model)
    {
        var role = HttpContext.Session.GetString("RoleName");
        if (role != "Staff" && role != "Lecturer") return RedirectToAction("Login", "Auth");

        await _api.UpdateAccountAsync(model);
        HttpContext.Session.SetString("AccountName", model.AccountName ?? "");
        TempData["Success"] = "Profile updated successfully!";
        return RedirectToAction("Index");
    }
}