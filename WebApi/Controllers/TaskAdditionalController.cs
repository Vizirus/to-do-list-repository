using BusinessLayer.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.BusinessLayer.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskAdditionalController : ControllerBase
{
    private readonly ITaskAdditionalService service;

    public TaskAdditionalController(ITaskAdditionalService service)
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
    public async Task<IActionResult> Add([FromBody] TaskCommentsModel model)
    {
        var result = await this.service.AddAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed !");
        }

        return this.Ok("Added new comment!");
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TaskCommentsModel model)
    {
        var result = await this.service.UpdateAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed!");
        }

        return this.Ok("Updated comment!");
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

    // TaskStatuses CRUD endpoints
    [HttpPost("status")]
    public async Task<IActionResult> AddTaskStatus([FromBody] TaskStatusesModel model)
    {
        var result = await this.service.AddTaskStatusAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed!");
        }

        return this.Ok("Added new task status!");
    }

    [HttpPut("status")]
    public async Task<IActionResult> UpdateTaskStatus([FromBody] TaskStatusesModel model)
    {
        var result = await this.service.UpdateTaskStatusAsync(model);
        if (!result)
        {
            return this.BadRequest("Validation failed!");
        }

        return this.Ok("Updated task status!");
    }

    [HttpDelete("status/{id:int}")]
    public async Task<IActionResult> DeleteTaskStatus(int id)
    {
        var result = await this.service.DeleteTaskStatusAsync(id);
        if (!result)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpGet("status/{id:int}")]
    public async Task<IActionResult> GetTaskStatusById(int id)
    {
        var result = await this.service.GetTaskStatusByIdAsync(id);
        if (result == null)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }

    [HttpGet("status")]
    public async Task<IActionResult> GetAllTaskStatuses()
    {
        var result = await this.service.GetAllTaskStatusesAsync();
        if (result == null)
        {
            return this.NotFound();
        }

        return this.Ok(result);
    }
}
