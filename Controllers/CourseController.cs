using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Contentful.Core;
using Contentful.Core.Search;
using miniproject.Models;
using miniproject.Models.Contentful;
using Newtonsoft.Json;

namespace miniproject.Controllers
{
    // [Authorize]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IContentfulClient _contentful;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signManager;

        public CourseController(
            ApplicationDbContext context, 
            IContentfulClient contentful, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            _context = context;
            _contentful = contentful;
            _userManager = userManager;
            _roleManager = roleManager;
            _signManager = signInManager;
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

        // GET: Course
        public async Task<IActionResult> Index()
        {
            // return View(await _context.Course.ToListAsync());
            var courses = await _contentful.GetEntriesByType<CourseType>("courses");
            var teachers = await _contentful.GetEntriesByType<Teacher>("teacher");
            ViewBag.teachers = teachers;
            return View(courses);
        }

        public async Task<IActionResult> GetCourse(string id)
        {
            // var lesson = await _client.GetEntry<Lesson>(id);
            var queryBuilder = QueryBuilder<Lesson>.New.Include(2)
                .ContentTypeIs("lesson")
                .FieldEquals(f => f.sku, id);
            var lesson = await _contentful.GetEntries(queryBuilder);
            ViewData["examplevideo"] = lesson.Items.First().linkyoutube?.Replace("youtu.be","youtube.com/embed");
            
            if(_signManager.IsSignedIn(User) && await OrderCheck(id))
            {
                Order myOrder = await GetOrderBySku(id);
                if(myOrder.PaymentState == Orders.PAID) {
                    return View("GetCourseB", lesson.Items.First());
                } else {
                    return RedirectToAction("Course", "Payment", new { id = myOrder.Id});
                }
            }
            else
                return View(lesson.Items.First<Lesson>());
        }

        public async Task<IActionResult> Teachers()
        {
            var teachers = await _contentful.GetEntriesByType<Teacher>("teacher");
            return View(teachers);
        }

        public async Task<IActionResult> Teacher(string id)
        {
            var queryBuilder = QueryBuilder<Teacher>.New.Include(2)
                .ContentTypeIs("teacher")
                .FieldEquals(f => f.teacherId, id);
            var teacher = await _contentful.GetEntries(queryBuilder);

            var queryCourse = QueryBuilder<Lesson>.New.Include(2)
                .ContentTypeIs("lesson")
                .FieldEquals("fields.teacher.sys.contentType.sys.id", "teacher")
                .FieldEquals("fields.teacher.fields.teacherId", id);
            var courses = await _contentful.GetEntries(queryCourse);
            ViewBag.courses = courses;
            // Console.WriteLine(JsonConvert.SerializeObject(courses));

            return View(teacher.Items.First());
        }

        public async Task<IActionResult> Types()
        {
            var courseTypes = await _contentful.GetEntriesByType<CourseType>("courses");
            return View(courseTypes);
        }

        public async Task<IActionResult> Type(string id)
        {
            var queryBuilder = QueryBuilder<CourseType>.New.Include(2)
                .ContentTypeIs("courses")
                .FieldEquals(f => f.name, id);
            var courseType = await _contentful.GetEntries(queryBuilder);

            var queryCourse = QueryBuilder<Lesson>.New.Include(2)
                .ContentTypeIs("lesson")
                .FieldEquals("fields.courses.sys.id", courseType.First().Sys.Id);
            var courses = await _contentful.GetEntries(queryCourse);
            ViewBag.courses = courses;

            // Console.WriteLine(JsonConvert.SerializeObject(courses));

            return View(courseType.Items.First());
        }















































        // GET: Course/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price")] TheCourse course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price")] TheCourse course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
