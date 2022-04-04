using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class CategoryForManagerController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryForManagerController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            var categories = _db.Categories
                             .OrderBy(c => c.CategoryID)
                             .ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            if (ModelState.IsValid)
            {
                return View(category);
            }
            else
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        public IActionResult Delete(int id)
        {
            var courseCategory = _db.Categories.FirstOrDefault(t => t.CategoryID == id);
            if (courseCategory != null)
            {
                _db.Categories.Remove(courseCategory);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update( int id)
        {
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            var category = _db.Categories.FirstOrDefault(t => t.CategoryID == id);
            if (category != null)
            {
                return View(category);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Update(Category category)
        {
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            if (ModelState.IsValid)
            {
                return View(category);
            }
            else
            {
                _db.Entry(category).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
