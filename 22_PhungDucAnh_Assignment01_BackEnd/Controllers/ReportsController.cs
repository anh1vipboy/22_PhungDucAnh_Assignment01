using Microsoft.AspNetCore.Mvc;
using _22_PhungDucAnh_Assignment01_BackEnd.Repositories;

namespace _22_PhungDucAnh_Assignment01_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly INewsArticleRepository _newsRepo;

    public ReportsController(INewsArticleRepository newsRepo)
    {
        _newsRepo = newsRepo;
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var start = startDate ?? DateTime.MinValue;
        var end = endDate ?? DateTime.MaxValue;

        var report = await _newsRepo.GetReportAsync(start, end);
        return Ok(new
        {
            TotalArticles = report.Count,
            StartDate = start,
            EndDate = end,
            Articles = report.Select(a => new
            {
                a.NewsArticleId,
                a.NewsTitle,
                a.Headline,
                a.CreatedDate,
                CategoryName = a.Category?.CategoryName ?? string.Empty,
                CreatedByName = a.CreatedBy?.AccountName ?? string.Empty,
                a.NewsStatus
            })
        });
    }
}