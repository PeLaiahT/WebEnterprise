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
            var categories = categoryRepo.GetListCategory();
            return View(categories);
        }
        
        [Authorize(Roles = "Assurance")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Assurance")]
        [HttpPost]
        public IActionResult Create(Category category)
        {
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
            var courseCategory = _db.Categories.FirstOrDefault(t => t.CategoryID == id);
            if (courseCategory != null)
            {
                _db.Categories.Remove(courseCategory);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Assurance")]
        [HttpGet]
        public IActionResult Update( int id)
        {
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
