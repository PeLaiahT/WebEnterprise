using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class IdeaController : Controller
    {
        private readonly ApplicationDbContext _db;
        private List<SelectListItem> GetDropDownCategory()
        {
            var categories = _db.Categories.Select(c => new SelectListItem { Text = c.NameCategory, Value = c.CategoryID.ToString() }).ToList();
            return categories;
        }
        public IdeaController(ApplicationDbContext db)
        {
            _db = db;
        }
        /*private void ViewComments()
        {
            var commments = (from c in _db.Comments
                             join i in _db.Ideas on c.IdeaID equals i.IdeaID
                             orderby c.CreateAt descending
                             select new Comment
                             {
                                 CommentID = c.CommentID,
                                 Content = c.Content,
                             }).ToList();
            ViewBag.Comments = commments;
        }*/
        public IActionResult Index()
        {
            IEnumerable<Idea> ideas = _db.Ideas.OrderByDescending(i => i.CreateAt);
            //ViewComments();
            return View(ideas);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.categories = GetDropDownCategory();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Idea idea)
        {
            ViewBag.categories = GetDropDownCategory();
            if (!ModelState.IsValid)
            {
                _db.Ideas.Add(idea);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(idea);
            }
           
        }
        public IActionResult Delete(int id)
        {
            var courseCategory = _db.Ideas.FirstOrDefault(t => t.IdeaID == id);
            if (courseCategory != null)
            {
                _db.Ideas.Remove(courseCategory);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            ViewBag.categories = GetDropDownCategory();
            var idea = _db.Ideas.FirstOrDefault(t => t.IdeaID == id);
            if (idea != null)
            {
                return View(idea);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Update(Idea idea)
        {
            ViewBag.categories = GetDropDownCategory();
            if (ModelState.IsValid)
            {
                return View(idea);
            }
            else
            {
                _db.Entry(idea).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

    }
}
