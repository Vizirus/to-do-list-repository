using BusinessLayer.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.BusinessLayer.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListController : ControllerBase
{
    private readonly IListService service;

    public ListController(IListService service)
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
    public async Task<IActionResult> Add([FromBody] ListsModel model)
    {
        var result = await this.service.AddAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed !");
        }

        return this.Ok("Added new list!");
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ListsModel model)
    {
        var result = await this.service.UpdateAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed!");
        }

        return this.Ok("Updated list!");
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

    // SharedLists CRUD endpoints
    [HttpPost("shared")]
    public async Task<IActionResult> AddSharedList([FromBody] SharedListsModel model)
    {
        var result = await this.service.AddSahredListAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed!");
        }

        return this.Ok("Added new shared list!");
    }

    [HttpPut("shared")]
    public async Task<IActionResult> UpdateSharedList([FromBody] SharedListsModel model)
    {
        var result = await this.service.UpdateSahredListgAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed!");
        }

        return this.Ok("Updated shared list!");
    }

    [HttpDelete("shared/{id:int}")]
    public async Task<IActionResult> DeleteSharedList(int id)
    {
        var result = await this.service.DeleteSahredListAsync(id);
        if (!result)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpGet("shared/{id:int}")]
    public async Task<IActionResult> GetSharedListById(int id)
    {
        var result = await this.service.GetSahredListByIdAsync(id);
        if (result == null)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpGet("shared")]
    public async Task<IActionResult> GetAllSharedLists()
    {
        var result = await this.service.GetAllTSahredListsAsync();
        if (result == null)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }
}
