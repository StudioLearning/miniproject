using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using miniproject.Models;
using miniproject.Models.Contentful;
using Contentful.Core;
using Contentful.Core.Search;
using Newtonsoft.Json;

namespace miniproject.Controllers;

[Authorize]
public class PaymentController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IContentfulClient _contentful;
    // private readonly UserManager<ApplicationUser> _userManager;

    public PaymentController(
        ILogger<HomeController> logger, 
        IContentfulClient contentful,
        ApplicationDbContext context
    )
    {
        _logger = logger;
        _context = context;
        _contentful = contentful;
    }

    public async Task<IActionResult> Index()
    {
        // var lesson = await _client.GetEntries<Lesson>();
        // var less = await _client.GetEntriesByType<dynamic>("lesson");
        // Console.WriteLine(less.First().image.fields.file.url);
        var lesson = await _contentful.GetEntriesByType<Lesson>("lesson");
        // Console.WriteLine(lesson.First().content);
        var queryBuilder = QueryBuilder<dynamic>.New.Include(2)
            .ContentTypeIs("config")
            .FieldEquals("sys.id", "2hAOEfjAjojqqm6ZuIHtEP");
        var website = await _contentful.GetEntries(queryBuilder);
        ViewBag.w = website.Items.First();
     
        ViewData["fullwidth"] = "Yes is full";
        return View(lesson);
    }

    // string id = orderId
    public async Task<IActionResult> Course(string id)
    {
        if (id == null)
        {
            return NotFound();
        }
        int TheId = Convert.ToInt32(id);
        Order order = _context.Order.FirstOrDefault(m => m.Id == TheId);
        if (order == null)
        {
            return NotFound();
        }
        var queryBuilder = QueryBuilder<Lesson>.New.Include(2)
            .ContentTypeIs("lesson")
            .FieldEquals(f => f.sku, order.Sku);
        var lesson = await _contentful.GetEntries(queryBuilder);
        ClaimsPrincipal currentUser = this.User;
        var currentUserID = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
        ViewBag.currentUserID = currentUserID;
        ViewBag.order = order;
        return View(lesson.Items.First());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
