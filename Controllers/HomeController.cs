using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using miniproject.Models;
using miniproject.Models.Contentful;
using Contentful.Core;
using Contentful.Core.Search;
using Newtonsoft.Json;

namespace miniproject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IContentfulClient _contentful;

    public HomeController(ILogger<HomeController> logger, IContentfulClient contentful)
    {
        _logger = logger;
        _contentful = contentful;
    }

    public async Task<IActionResult> Index()
    {
        // var lesson = await _client.GetEntries<Lesson>();
        // var lesson = await _contentful.GetEntriesByType<Lesson>("lesson");
        var queryLessonBuilder = QueryBuilder<Lesson>
            .New.Limit(4)
            .Include(2)
            .ContentTypeIs("lesson");
        var lesson = await _contentful.GetEntries(queryLessonBuilder);

        // Console.WriteLine(lesson.First().content);
        var queryBuilder = QueryBuilder<Website>.New.Include(2)
            .ContentTypeIs("config")
            .FieldEquals("sys.id", "2hAOEfjAjojqqm6ZuIHtEP");
        var website = await _contentful.GetEntries(queryBuilder);
        ViewBag.website = website.Items.First();
     
        ViewData["fullwidth"] = "Yes is full";
        return View(lesson);
    }

    public async Task<String> Test(string id)
    {
        //var lesson = await _client.GetEntry<dynamic>("2hAOEfjAjojqqm6ZuIHtEP");
        var queryBuilder = QueryBuilder<dynamic>.New.Include(2)
            //.ContentTypeIs("lesson")
            .FieldEquals("sys.id", "2hAOEfjAjojqqm6ZuIHtEP");
        var lesson = await _contentful.GetEntries(queryBuilder);
        return JsonConvert.SerializeObject(lesson);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Me() {
        ViewData["fullwidth"] = "Yes is full";
        return View();
    }

    public IActionResult Term() {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
