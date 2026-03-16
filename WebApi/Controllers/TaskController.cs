using BusinessLayer.BusinessLayer.Interfaces;
using BusinessLayer.BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService service;

    public TaskController(ITaskService service)
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
    public async Task<IActionResult> Add([FromBody] TaskModel model)
    {
        var result = await this.service.AddAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed!");
        }

        return this.Ok("Added new task!");
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TaskModel model)
    {
        var result = await this.service.UpdateAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed!");
        }

        return this.Ok("Updated task!");
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
}
