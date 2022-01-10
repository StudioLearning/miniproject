using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using miniproject.Models;
using miniproject.Models.Report;
using miniproject.Models.Contentful;
using Microsoft.AspNetCore.Authorization;
using Contentful.Core;
using Contentful.Core.Search;

namespace miniproject.Controllers;

[Authorize(Roles= "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminController> _logger;
    private readonly IContentfulClient _contentful;

    public AdminController(
        ILogger<AdminController> logger, 
        ApplicationDbContext context,
        IContentfulClient contentful
    )
    {
        _context = context;
        _logger = logger;
        _contentful = contentful;
    }

    private string[] monthNames =
    {
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December"
    };

    private decimal calTotalIncome4Month(int month, int year){
        var DataReport = _context.Order.ToList()
            .FindAll(p =>
                (p.PaidDate.Month == month) &&
                (p.PaidDate.Year == year) &&
                (p.PaymentState == Orders.PAID)
            );

        return DataReport.Sum(p => p.Price);
    }

    private async Task<decimal> calTotalIncome4Teacher(string teacherId, int month, int year){
        var queryCourse = QueryBuilder<Lesson>.New.Include(2)
                .ContentTypeIs("lesson")
                .FieldEquals("fields.teacher.sys.contentType.sys.id", "teacher")
                .FieldEquals("fields.teacher.fields.teacherId", teacherId);
        var courses = await _contentful.GetEntries(queryCourse);
        
        var DataReport = _context.Order.ToList()
            .FindAll(p =>
                (p.PaidDate.Month == month) &&
                (p.PaidDate.Year == year) &&
                (p.PaymentState == Orders.PAID)
            );

        decimal incomeTotal = 0;
        foreach (var course in courses)
        {
            foreach (var rp in DataReport)
            {
                if(rp.Sku == course.sku){
                    incomeTotal += rp.Price;
                }
            }
            
        }

        return incomeTotal;
    }

    private Report4Years Report4Y(int year)
    {
        List<Report4Year> report = new List<Report4Year>();
        int index4month = 1;
        foreach (var month in monthNames)
        {
            if(year == DateTime.Now.Year && index4month > DateTime.Now.Month){
                continue;
            }
            report.Add(new Report4Year() {
                name = monthNames[index4month -1],
                indexName = index4month,
                link = Url.ActionLink("Report4Month", "Admin", new { month = index4month, year = year }),
                income = calTotalIncome4Month(index4month, year),
            });
            index4month++;
        }
        return new Report4Years() {
            report = report,
            year = year,
        };
    }

    public IActionResult Index()
    {
        return View("Report4Year", 
            Report4Y(DateTime.Now.Year)
        );
    }

    public IActionResult Report4Year(int year)
    {
        return View(Report4Y(year));
    }

    public async Task<IActionResult> Report4Month(int month, int year)
    {
        var teachers = await _contentful.GetEntriesByType<Teacher>("teacher");
        var DataReport = _context.Order
            .Where(p => 
                (p.PaidDate.Month == month) &&
                (p.PaidDate.Year == year) &&
                (p.PaymentState == Orders.PAID)
            );
        List<Report4Month> report = new List<Report4Month>();
        foreach (var teacher in teachers)
        {
            decimal teacherIncome = await calTotalIncome4Teacher(teacher.teacherId, month, year);
            if(teacherIncome > 0) {
                report.Add(
                    new Report4Month() {
                        Id = teacher.teacherId,
                        name = teacher.teacherName,
                        link = Url.ActionLink(
                            "Report4Teacher",
                            "Admin",
                            new { teacherId = teacher.teacherId, month = month, year = year }
                        ),
                        income = teacherIncome,
                        color = String.Format("#{0:X6}", new Random().Next(0x1000000))
                    }
                );
            }
        }

        ViewBag.month = monthNames[month - 1];
        ViewBag.year = year;
        return View(report);
    }

    public async Task<IActionResult> Report4Teacher(string teacherId, int month, int year)
    {
        var queryBuilder = QueryBuilder<Teacher>.New.Include(2)
                .ContentTypeIs("teacher")
                .FieldEquals(f => f.teacherId, teacherId);
        var teachers = await _contentful.GetEntries(queryBuilder);
        var teacher = teachers.Items.FirstOrDefault();

        var queryCourse = QueryBuilder<Lesson>.New.Include(2)
                .ContentTypeIs("lesson")
                .FieldEquals("fields.teacher.sys.contentType.sys.id", "teacher")
                .FieldEquals("fields.teacher.fields.teacherId", teacherId);
        var courses = await _contentful.GetEntries(queryCourse);

        var DataReport = _context.Order.ToList().FindAll(p => 
                (p.PaidDate.Month == month) &&
                (p.PaidDate.Year == year) &&
                (p.PaymentState == Orders.PAID)
            );
        
        List<Report4TeacherReport> report = new List<Report4TeacherReport>();

        foreach (var course in courses)
        {
            decimal incomeTotal = 0;
            int quantityTotal = 0;
            foreach (var rp in DataReport)
            {
                if(rp.Sku == course.sku){
                    incomeTotal += rp.Price;
                    quantityTotal++;
                }
            }
            if(incomeTotal > 0) {
                report.Add(
                    new Report4TeacherReport() {
                        sku = course.sku,
                        name = course.name,
                        quantity = quantityTotal,
                        income = incomeTotal,
                        color = String.Format("#{0:X6}", new Random().Next(0x1000000))
                    }
                );
            }
        }

        decimal revenueTotal = report.Sum(p => p.income);
        decimal TheTax = revenueTotal * 0.07M;
        decimal TheRevenue = revenueTotal - TheTax;

        ViewBag.month = monthNames[month - 1];
        ViewBag.year = year;
        return View(
            new Report4Teacher() {
                teacher = teacher,
                report = report,
                revenueBeforeTax = revenueTotal,
                tax = TheTax,
                revenue = TheRevenue,
                incomeTotal = TheRevenue * 0.7M,
            }
        );
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
