using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using miniproject.Models;
using Microsoft.AspNetCore.Authorization;

namespace miniproject.Controllers;

[Authorize(Roles= "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        ILogger<AdminController> logger, 
        ApplicationDbContext context
    )
    {
        _context = context;
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
