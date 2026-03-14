using DataLayer.DataLayer.ContextData;
using DataLayer.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    private readonly ToDoListAppDbContext context;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ToDoListAppDbContext context)
    {
        this._logger = logger;
        this.context = context;
    }

    [HttpGet]
    public List<DataLayer.DataLayer.Entities.Task> Index()
    {
        return this.context.tasks.ToList();
    }
}
