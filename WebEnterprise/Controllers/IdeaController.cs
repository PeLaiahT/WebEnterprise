using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Common;
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
        public async Task<IActionResult> Index(int? pageIndex)
        {
            var listIdea = _db.Ideas.Include(i => i.Category)
                .Include(i => i.IdeaUser)
                .Include(i => i.Documments)
                .OrderByDescending(i => i.CreateAt);

            return View(await PaginatedList<Idea>.CreateAsync(listIdea, pageIndex ?? 1, 5));
        }
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> IndexUser(int? pageIndex)
        {
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            var listIdea = _db.Ideas.Include(i => i.Category)
                .Include(i => i.IdeaUser)
                .Include(i => i.Documments)
                .OrderByDescending(i => i.CreateAt);
            return View(await PaginatedList<Idea>.CreateAsync(listIdea, pageIndex ?? 1, 5));
        }
        [HttpGet]
        public IActionResult EditClosureDate(int id)
        {
            var idea = _db.Ideas.Where(i => i.IdeaID == id).FirstOrDefault();
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
        public IActionResult EditClosureDate(Idea idea)
        {
            if (!ModelState.IsValid)
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
        public async Task<IActionResult> CreateAsync(DocsIdea idea2, List<IFormFile> postedFile)
        {
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
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
                    if (postedFile != null)
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
                ViewBag.categories = GetDropDownCategory();
                return View(idea2);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Download(int id)
        {
            var provider = new FileExtensionContentTypeProvider();
            var documment = await _db.Documments.FindAsync(id);
            if (documment == null)
            {
                return NotFound();
            }
            var file = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "MyFiles", documment.FileName);
            string contentType;
            if (!provider.TryGetContentType(file, out contentType))
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
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
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
            var idea = _db.Ideas.Include(i => i.Documments).FirstOrDefault(t => t.IdeaID == id);
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
        public async Task<IActionResult> Update(Idea idea, List<IFormFile> postedFile)
        {
            ViewBag.categories = GetDropDownCategory();
            if (!ModelState.IsValid)
            {
                return View(idea);
            }
            else
            {
                foreach (IFormFile f in postedFile)
                {
                    if (postedFile != null)
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
                        IdeaID = idea.IdeaID
                    };
                    _db.Documments.Add(docs);
                    _db.SaveChanges();
                }
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
            idea.View++;
            _db.SaveChanges();
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
        public IActionResult MostLike()
        {

            var mostLike = _db.Ideas.Max(i => i.Likecount);
            var idea = _db.Ideas.
                Include(i => i.IdeaUser).
                Include(i => i.Category).
                Include(i => i.Documments).
                Include(i => i.Comments).ThenInclude(c => c.CommentUser)
                .Where(i => i.Likecount == mostLike).FirstOrDefault();
            return View(idea);
        }
        public IActionResult MostView()
        {
            var mostView = _db.Ideas.Max(i => i.View);
            var idea = _db.Ideas.Include(i => i.IdeaUser).
                Include(i => i.Category).
                Include(i => i.Documments).
                Include(i => i.Comments).ThenInclude(c => c.CommentUser).Where(i => i.View == mostView).FirstOrDefault();

            return View(idea);
        }
        public IActionResult MostLikeStaff()
        {

            var mostLike = _db.Ideas.Max(i => i.Likecount);
            var idea = _db.Ideas.
                Include(i => i.IdeaUser).
                Include(i => i.Category).
                Include(i => i.Documments).
                Include(i => i.Comments).ThenInclude(c => c.CommentUser)
                .Where(i => i.Likecount == mostLike).FirstOrDefault();
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            return View(idea);
        }
        public IActionResult MostViewStaff()
        {
            var mostView = _db.Ideas.Max(i => i.View);
            var idea = _db.Ideas.Include(i => i.IdeaUser).
                Include(i => i.Category).
                Include(i => i.Documments).
                Include(i => i.Comments).ThenInclude(c => c.CommentUser).Where(i => i.View == mostView).FirstOrDefault();
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.image = user.FileName;
            return View(idea);
        }
        public IActionResult DeleteDoc(int id)
        {
            var doc = _db.Documments.FirstOrDefault(t => t.DocummentID == id);
            if (doc != null)
            {
                _db.Documments.Remove(doc);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Dashboard(int? id)
        {
            var department = _db.Departments.ToList();
            var a = (from d in _db.Departments
                     where d.DepartmentID == id
                     join u in _db.CustomUsers on d.DepartmentID equals u.DepartmentID
                     join i in _db.Ideas on u.Id equals i.IdeaUserID
                     select new Idea
                     {
                         Title = i.Title
                     }).ToList();
            ViewBag.ToTal = a.Count();
            return View(department);
        }
    }
}
