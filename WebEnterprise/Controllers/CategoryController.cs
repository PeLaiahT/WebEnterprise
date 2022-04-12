using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Respon;
namespace WebEnterprise.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ICategoryRepo categoryRepo;
        public CategoryController(ApplicationDbContext db , ICategoryRepo _categoryRepo)
        {
            _db = db;
            categoryRepo = _categoryRepo;
        }
        public IActionResult Index()
        {
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            var categories = categoryRepo.GetListCategory();
            return View(categories);
        }
        
        [Authorize(Roles = "Assurance")]
        [HttpGet]
        public IActionResult Create()
        {
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            return View();
        }
        [Authorize(Roles = "Assurance")]
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
        [Authorize(Roles = "Assurance")]
        public IActionResult Delete(int id)
        {

            var category = _db.Categories.Include(d => d.Ideas).FirstOrDefault(t => t.CategoryID == id);
            if (category != null)
            {
                if (category.Ideas.Count() > 0)
                {
                    TempData["Error"] = $"Cannot delete {category.NameCategory} category";
                    return RedirectToAction("Index");
                }
                else
                {
                    _db.Categories.Remove(category);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
                return RedirectToAction("Index"); ;
        }
        [Authorize(Roles = "Assurance")]
        [HttpGet]
        public IActionResult Update( int id)
        {
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            var category = categoryRepo.GetUpdate(id);
            if (category != null)
            {
                return View(category);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "Assurance")]
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
