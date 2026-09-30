using Microsoft.AspNetCore.Mvc;
using _22_PhungDucAnh_Assignment01_FrontEnd.Services;

namespace _22_PhungDucAnh_Assignment01_FrontEnd.Controllers;

public class ReportsController : Controller
{
    private readonly ApiService _api;

    public ReportsController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
    {
        if (HttpContext.Session.GetString("RoleName") != "Admin")
        {
            return RedirectToAction("Login", "Auth");
        }

        var report = await _api.GetReportAsync(startDate, endDate);
        report.StartDate = startDate;
        report.EndDate = endDate;

        return View(report);
    }
}