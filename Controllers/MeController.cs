using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using miniproject.Models;

namespace miniproject.Controllers;

public class MeController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public MeController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
