using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    public HomeController(ILogger<HomeController> logger)
    {
        this.logger = logger;
    }

    public IActionResult Index()
    {
        if (this.User.Identity?.IsAuthenticated == true)
        {
            return this.RedirectToAction(actionName: "Index", controllerName: "Lists");
        }

        return this.RedirectToAction(actionName: "Login", controllerName: "Account");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        this.logger.LogError(1, "The error have happend during this request!");
        return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}
