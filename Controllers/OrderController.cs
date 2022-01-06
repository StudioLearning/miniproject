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
using System.Net;
using System.Text;

namespace miniproject.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IContentfulClient _contentful;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signManager;

    public OrderController(
        ILogger<HomeController> logger, 
        ApplicationDbContext context,
        IContentfulClient contentful,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager
    )
    {
        _logger = logger;
        _context = context;
        _contentful = contentful;
        _userManager = userManager;
        _roleManager = roleManager;
        _signManager = signInManager;
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

    public async Task<IActionResult> Course(string id)
    {
        var queryBuilder = QueryBuilder<Lesson>.New.Include(2)
            .ContentTypeIs("lesson")
            .FieldEquals(f => f.sku, id);
        var lesson = await _contentful.GetEntries(queryBuilder);
        return View(lesson.Items.First());
    }

    private void LineNotify(string msg)
        {
            string token = "lccx6Dn2jpNthqF0rPL3jjfboiHwjkPeBZoDsYduuvr";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", msg);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer "+token);

                using (var stream = request.GetRequestStream())stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> HookPayPal([FromBody] Root id)
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        await Task.Run(() => Console.WriteLine(JsonConvert.SerializeObject(id)));
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();

        // await Task.Run(() => LineNotify(JsonConvert.SerializeObject(id).ToString()));
        // Update database order here!

        return Ok();
    }

    // [HttpPost]
    public async Task<IActionResult> CreateOrder(string id)
    {
        var queryBuilder = QueryBuilder<Lesson>.New.Include(2)
            .ContentTypeIs("lesson")
            .FieldEquals(f => f.sku, id);
        var lesson = await _contentful.GetEntries(queryBuilder);
        Console.WriteLine("----------------------------------");
        Console.WriteLine(id);
        Console.WriteLine(lesson.Items.First().sku);
        Console.WriteLine(lesson.Items.First().Sys.Id);
        Console.WriteLine(_userManager.GetUserId(User));
        Console.WriteLine((DateTimeOffset)DateTime.UtcNow);
        Console.WriteLine(lesson.Items.First().price);
        Console.WriteLine("----------------------------------");

        var OrderId = 125;

        var order = new Order {
            Id = OrderId, 
            Sku = lesson.Items.First().sku,
            ContentfulID = lesson.Items.First().Sys.Id,
            UserId = _userManager.GetUserId(User),
            CreateDate = DateTime.UtcNow,
            Price = Convert.ToDecimal(lesson.Items.First().price),
            PaymentState = Orders.PENDING
        };

        if (ModelState.IsValid)
        {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Course", "Payment", new { id = OrderId});
        }
        return Ok();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
