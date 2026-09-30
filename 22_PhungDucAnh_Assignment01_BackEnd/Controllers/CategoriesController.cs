using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;
using _22_PhungDucAnh_Assignment01_BackEnd.Repositories;

namespace _22_PhungDucAnh_Assignment01_BackEnd.Controllers;

public class CategoriesController : ODataController
{
    private readonly ICategoryRepository _categoryRepo;

    public CategoriesController(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    [HttpGet]
    [EnableQuery]
    public IQueryable<Category> Get()
    {
        return _categoryRepo.GetCategories();
    }

    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] short key)
    {
        var category = await _categoryRepo.GetCategoryByIdAsync(key);
        if (category == null) return NotFound($"Category with ID {key} not found.");
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Category category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var created = await _categoryRepo.AddCategoryAsync(category);
        return Created(created);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromRoute] short key, [FromBody] Category category)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        category.CategoryId = key;
        var updated = await _categoryRepo.UpdateCategoryAsync(category);
        if (updated == null) return NotFound($"Category with ID {key} not found.");
        return Updated(updated);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] short key)
    {
        bool hasNews = await _categoryRepo.HasNewsArticlesAsync(key);
        if (hasNews)
        {
            return BadRequest("Cannot delete Category because it already has News Articles.");
        }

        bool deleted = await _categoryRepo.DeleteCategoryAsync(key);
        if (!deleted) return NotFound($"Category with ID {key} not found.");
        return NoContent();
    }
}