using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Models.DTO;

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

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var listIdea = _db.Ideas.Include(i => i.Category)
                .Include(i => i.IdeaUser)
                .Include(i => i.Documments)
                .OrderByDescending(i => i.CreateAt).ToList();
            return View(listIdea);
        }
        [HttpGet]
        public IActionResult EditClosureDate(int id)
        {
            var date = _db.Ideas.FirstOrDefault(t => t.IdeaID == id);
            ViewBag.categories = GetDropDownCategory();
            if (date != null)
            {
                return View(date);
            }
            else
            {
                return RedirectToAction("Index");
            }
           
        }
        [HttpPost]
        public IActionResult EditClosureDate(Idea date)
        {
            ViewBag.categories = GetDropDownCategory();
            if (!ModelState.IsValid)
            {
                return View(date);
            }
            else
            {
                _db.Entry(date).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "Staff")]
        public IActionResult IndexUser()
        {
            var listIdea = _db.Ideas.Include(i => i.Category)
                .Include(i => i.IdeaUser)
                .Include(i => i.Documments)
                .OrderByDescending(i => i.CreateAt).ToList();
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            return View(listIdea);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var un = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(un)).FirstOrDefault();
            ViewBag.image = user.FileName;
            ViewBag.categories = GetDropDownCategory();
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(DocsIdea idea2, List <IFormFile> postedFile)
        {
            var un = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(un)).FirstOrDefault();
            ViewBag.image = user.FileName;
            ViewBag.categories = GetDropDownCategory();
            
            if (ModelState.IsValid)
            {
                var idea1 = new Idea
                {
                    IdeaUserID = User.Identity.GetUserId(),
                    Content = idea2.Content,
                    Title = idea2.Title,
                    CategoryID = idea2.CategoryID,
                };
                _db.Ideas.Add(idea1);
                _db.SaveChanges();
                foreach (IFormFile f in postedFile)
                {
                    if(postedFile!= null)
                    {
                    //chi dinh duong dan se luu
                    string fullPath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "MyFiles", f.FileName);

                        using (var file = new FileStream(fullPath, FileMode.Create))
                        {
                           await f.CopyToAsync(file);
                        }
                    }
                    var docs = new Documment
                    {
                        FileName = f.FileName,
                        ContentType = f.ContentType,
                        IdeaID = idea1.IdeaID
                    };
                    _db.Documments.Add(docs);
                    _db.SaveChanges();
                }
                return RedirectToAction("IndexUser");
            }
            else
            {
                return View(idea2);
            } 
        }

        [HttpPost]
        public async Task<IActionResult> Download(int id)
        {
            var provider = new FileExtensionContentTypeProvider();
            var documment = await _db.Documments.FindAsync(id);
            if(documment == null)
            {
                return NotFound();
            }
            var file = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "MyFiles", documment.FileName);
            string contentType;
            if(!provider.TryGetContentType(file ,out contentType))
            {
                contentType = "application/octet-stream";
            }
            byte[] fileBytes;
            if (System.IO.File.Exists(file)) 
            {
                fileBytes = System.IO.File.ReadAllBytes(file);
            }  
            else
            {
                return NotFound();
            }
            return File(fileBytes, contentType, documment.FileName);


        }
        public FileResult DownloadFile(string NameFile)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "MyFiles/") + NameFile;
            var provider = new FileExtensionContentTypeProvider();
            if(!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            byte[] bytes =  System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));

        }
        public IActionResult Delete(int id)
        {
            var idea = _db.Ideas.FirstOrDefault(t => t.IdeaID == id);
            if (idea != null)
            {
                _db.Ideas.Remove(idea);
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
        [Authorize(Roles = "Staff")]
        public IActionResult Detail(int id)
        {
            var idea = _db.Ideas.
                Include(i => i.Category).
                Include(i => i.IdeaUser).
                Include(i => i.Documments).
                FirstOrDefault(i => i.IdeaID == id);
            idea.Comments = _db.Comments.Where(i => i.IdeaID == id).Include(i => i.CommentUser).OrderByDescending(x => x.CreateAt).ToList();
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            return View(idea);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DetailForAdmin(int id)
        {
            var idea = _db.Ideas.
                Include(i => i.Category).
                Include(i => i.IdeaUser).
                Include(i => i.Documments).
                FirstOrDefault(i => i.IdeaID == id);
            idea.Comments = _db.Comments.Where(i => i.IdeaID == id).Include(i => i.CommentUser).OrderByDescending(x => x.CreateAt).ToList();
            return View(idea);
        }
    }
}
