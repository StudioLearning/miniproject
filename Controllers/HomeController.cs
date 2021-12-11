﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using miniproject.Models;

namespace miniproject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ViewData["fullwidth"] = "Yes is full";
        return View();
    }

    public IActionResult Course()
    {
        return View();
    }

    public IActionResult Payment()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
