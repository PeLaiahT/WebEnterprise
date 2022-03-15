using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        public async Task<IActionResult> CreateAsync(Idea idea, List <IFormFile> postedFile)
        {
            ViewBag.categories = GetDropDownCategory();
            if (!ModelState.IsValid)
            {
                foreach(IFormFile f in postedFile)
                {

                    using (var dataStream = new MemoryStream())
                    {
                        await f.CopyToAsync(dataStream);
                        idea.Documment = dataStream.ToArray();
                    }
                    if(postedFile!= null)
                    {
                    //chi dinh duong dan se luu
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "MyFiles", f.FileName);
                        ViewBag.fileName = f.FileName;
                    using(var file = new FileStream(fullPath,FileMode.Create))
                        {
                            f.CopyTo(file);
                        }

                    }
                }
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
