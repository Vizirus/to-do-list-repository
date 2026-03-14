using BusinessLayer.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.BusinessLayer.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagsService service;

    public TagsController(ITagsService service)
    {
        this.service = service;
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await this.service.GetByIdAsync(id);
        if (result == null)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await this.service.GetAllAsync();
        if (result == null)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] TagsModel model)
    {
        var result = await this.service.AddAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed !");
        }

        return this.Ok("Added new tag!");
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TagsModel model)
    {
        var result = await this.service.UpdateAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed!");
        }

        return this.Ok("Updated tag!");
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var result = await this.service.DeleteAsync(id);
        if (!result)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    // TaskTags CRUD endpoints
    [HttpPost("tasktag")]
    public async Task<IActionResult> AddTaskTag([FromBody] TaskTagsModel model)
    {
        var result = await this.service.AddTaskTagAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed!");
        }

        return this.Ok("Added new task tag!");
    }

    [HttpPut("tasktag")]
    public async Task<IActionResult> UpdateTaskTag([FromBody] TaskTagsModel model)
    {
        var result = await this.service.UpdateTaskTagAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed!");
        }

        return this.Ok("Updated task tag!");
    }

    [HttpDelete("tasktag/{id:int}")]
    public async Task<IActionResult> DeleteTaskTag(int id)
    {
        var result = await this.service.DeleteTaskTagAsync(id);
        if (!result)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpGet("tasktag/{id:int}")]
    public async Task<IActionResult> GetTaskTagById(int id)
    {
        var result = await this.service.GetTaskTagByIdAsync(id);
        if (result == null)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpGet("tasktag")]
    public async Task<IActionResult> GetAllTaskTags()
    {
        var result = await this.service.GetAllTaskTagsAsync();
        if (result == null)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }
}
