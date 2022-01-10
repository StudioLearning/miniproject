using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using miniproject.Models;
using miniproject.Models.Contentful;
using Contentful.Core;
using Contentful.Core.Images;
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

    private bool OrderExists(int id)
    {
        return _context.Order.Any(e => e.Id == id);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> HookPayPal([FromBody] Root id)
    {
        // Console.WriteLine();
        // Console.WriteLine();
        // Console.WriteLine();
        // await Task.Run(() => Console.WriteLine(JsonConvert.SerializeObject(id)));
        // Console.WriteLine();
        // Console.WriteLine();
        // Console.WriteLine();

        // await Task.Run(() => LineNotify(JsonConvert.SerializeObject(id).ToString()));
        // Update database order here!

        int customer_id = Convert.ToInt32(id.resource.purchase_units.First().custom_id);
        var orderBeforeUpdate = _context.Order.Where(order => order.Id == customer_id).First();

        if(orderBeforeUpdate != null) {
            Order order = orderBeforeUpdate;
            order.PaymentState = Orders.PAID;
            order.PaidDate = DateTime.UtcNow;
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return Ok();
                    }
                    else
                    {
                        throw;
                    }
                }
        }
        return Ok();
    }

    private int CreateOrderId() {
        Order order = _context.Order.OrderByDescending(p => p.Id).FirstOrDefault();
        int orderLastId = 1;
        if(order != null)
            orderLastId = order.Id+1;
        return orderLastId;
    }

    private async Task<Boolean> OrderCheck(string sku) {
        var currentID = _userManager.GetUserId(User);
        var Order = await _context.Order.ToListAsync();
        var MyOrder = Order.FindAll(order => order.UserId == currentID);
        return MyOrder.Any(order => order.Sku == sku);
    }

    private async Task<Order> GetOrderBySku(string sku) {
        var currentID = _userManager.GetUserId(User);
        var Order = await _context.Order.ToListAsync();
        return Order.Find(order => 
            (order.UserId == currentID) && (order.Sku == sku)
        );
    }

    private async Task<List<Order>> GetOrders() {
        var currentID = _userManager.GetUserId(User);
        var Order = await _context.Order.ToListAsync();
        return Order.FindAll(order => 
            order.UserId == currentID
        );
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

    public async Task<IActionResult> MyCourse()
    {
        List<Order> orders = await GetOrders();

        var queryBuilder = QueryBuilder<Lesson>.New.Include(2)
            .ContentTypeIs("lesson");
        var lessons = await _contentful.GetEntries(queryBuilder);

        var myCourse = new List<Lesson>();

        foreach (var order in orders)
        {
            foreach (var lesson in lessons)
            {
                if(lesson.sku == order.Sku && order.PaymentState == Orders.PAID)
                    myCourse.Add(lesson);
            }
        }

        return View(myCourse);
    }

    public async Task<IActionResult> MyOrder()
    {
        List<Order> orders = await GetOrders();

        return View(orders);
    }

    public async Task<IActionResult> Course(string id)
    {
        if(await OrderCheck(id))
        {
            Order myOrder = await GetOrderBySku(id);
            if(myOrder.PaymentState == Orders.PAID){
                return RedirectToAction("GetCourse", "Course", new {id = myOrder.Sku});
            } else {
                return RedirectToAction("Course", "Payment", new {id = myOrder.Id});
            }
        }
        var queryBuilder = QueryBuilder<Lesson>.New.Include(2)
            .ContentTypeIs("lesson")
            .FieldEquals(f => f.sku, id);
        var lessons = await _contentful.GetEntries(queryBuilder);
        var lesson = lessons.Items.FirstOrDefault();

        decimal tax = lesson.price * 0.07M;

        var imageUrlBuilder =
        ImageUrlBuilder.New().SetHeight(150).SetWidth(150).SetResizingBehaviour(ImageResizeBehaviour.Fill);
        string img = $"{lesson.image.File.Url}{imageUrlBuilder.Build()}";

        return View(
            new Cart(){
                lesson = lesson,
                tax = tax,
                price = lesson.price - tax,
                totalPrice = lesson.price,
                img = img,
            }
        );
    }

    // [HttpPost]
    public async Task<IActionResult> CreateOrder(string id)
    {
        if(await OrderCheck(id))
        {
            Order myOrder = await GetOrderBySku(id);
            return RedirectToAction("Course", "Payment", new {id = myOrder.Id});
        }

        var queryBuilder = QueryBuilder<Lesson>.New.Include(2)
            .ContentTypeIs("lesson")
            .FieldEquals(f => f.sku, id);
        var lesson = await _contentful.GetEntries(queryBuilder);
        // Console.WriteLine("----------------------------------");
        // Console.WriteLine(id);
        // Console.WriteLine(lesson.Items.First().sku);
        // Console.WriteLine(lesson.Items.First().Sys.Id);
        // Console.WriteLine(_userManager.GetUserId(User));
        // Console.WriteLine((DateTimeOffset)DateTime.UtcNow);
        // Console.WriteLine(lesson.Items.First().price);
        // Console.WriteLine("----------------------------------");

        var OrderId = CreateOrderId();

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

    public async Task<ActionResult<Orders>> ApiGetOrderPaymentState(int id) {
        if(id == null)
            return Orders.PENDING;

        Order order = _context.Order.FirstOrDefault(p => p.Id == id);
        if(order.PaymentState == Orders.PAID)
            return Orders.PAID;
        else
            return Orders.PENDING;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
