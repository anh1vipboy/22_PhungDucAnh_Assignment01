using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using _22_PhungDucAnh_Assignment01_BackEnd.DTOs;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;
using _22_PhungDucAnh_Assignment01_BackEnd.Repositories;

namespace _22_PhungDucAnh_Assignment01_BackEnd.Controllers;

public class NewsArticlesController : ODataController
{
    private readonly INewsArticleRepository _newsRepo;

    public NewsArticlesController(INewsArticleRepository newsRepo)
    {
        _newsRepo = newsRepo;
    }

    [HttpGet]
    [EnableQuery]
    public IQueryable<NewsArticle> Get()
    {
        return _newsRepo.GetNewsArticles();
    }

    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] string key)
    {
        var article = await _newsRepo.GetNewsArticleByIdAsync(key);
        if (article == null) return NotFound($"News Article with ID {key} not found.");
        return Ok(article);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] NewsArticleRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var article = new NewsArticle
        {
            NewsArticleId = request.NewsArticleId ?? string.Empty,
            NewsTitle = request.NewsTitle,
            Headline = request.Headline,
            NewsContent = request.NewsContent,
            NewsSource = request.NewsSource,
            CategoryId = request.CategoryId,
            NewsStatus = request.NewsStatus ?? true,
            CreatedById = request.CreatedById,
            UpdatedById = request.UpdatedById,
            CreatedDate = DateTime.UtcNow
        };

        var created = await _newsRepo.AddNewsArticleAsync(article, request.TagIds);
        return Created(created);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromRoute] string key, [FromBody] NewsArticleRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var article = new NewsArticle
        {
            NewsArticleId = key,
            NewsTitle = request.NewsTitle,
            Headline = request.Headline,
            NewsContent = request.NewsContent,
            NewsSource = request.NewsSource,
            CategoryId = request.CategoryId,
            NewsStatus = request.NewsStatus,
            UpdatedById = request.UpdatedById
        };

        var updated = await _newsRepo.UpdateNewsArticleAsync(article, request.TagIds);
        if (updated == null) return NotFound($"News Article with ID {key} not found.");
        return Updated(updated);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] string key)
    {
        bool deleted = await _newsRepo.DeleteNewsArticleAsync(key);
        if (!deleted) return NotFound($"News Article with ID {key} not found.");
        return NoContent();
    }
}