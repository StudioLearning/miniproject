using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using miniproject.Models;
using Microsoft.AspNetCore.Authorization;

namespace miniproject.Controllers;

[Authorize(Roles= "Admin")]
public class TestController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public TestController(ILogger<AuthController> logger)
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
