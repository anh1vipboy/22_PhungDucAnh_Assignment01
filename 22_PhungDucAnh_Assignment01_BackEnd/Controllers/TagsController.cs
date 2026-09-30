using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;
using _22_PhungDucAnh_Assignment01_BackEnd.Repositories;

namespace _22_PhungDucAnh_Assignment01_BackEnd.Controllers;

public class TagsController : ODataController
{
    private readonly ITagRepository _tagRepo;

    public TagsController(ITagRepository tagRepo)
    {
        _tagRepo = tagRepo;
    }

    [HttpGet]
    [EnableQuery]
    public IQueryable<Tag> Get()
    {
        return _tagRepo.GetTags();
    }

    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] int key)
    {
        var tag = await _tagRepo.GetTagByIdAsync(key);
        if (tag == null) return NotFound($"Tag with ID {key} not found.");
        return Ok(tag);
    }
}