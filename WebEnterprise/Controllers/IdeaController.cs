using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Data;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class IdeaController : Controller
    {
        private readonly ApplicationDbContext _db;
        private void ViewComments()
        {
            var commments = (from c in _db.Comments
                             join i in _db.Ideas on c.IdeaId equals i.IdeaID
                             orderby c.CreateAt descending
                             select new Comment
                             {
                                 IdCommment = c.IdCommment,
                                 Content = c.Content,
                             }).ToList();
            ViewBag.Comments = commments;
        }
        public IdeaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Idea> ideas = _db.Ideas.OrderByDescending(i => i.CreateAt);
            ViewComments();

            return View(ideas);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Idea idea)
        {
            if (ModelState.IsValid)
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
      
    }
}
