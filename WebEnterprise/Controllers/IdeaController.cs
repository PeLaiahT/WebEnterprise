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
        private List<SelectListItem> ListDepartment()
        {
            var department = _db.Departments.Select(c => new SelectListItem { Text = c.NameDepartment, Value = c.DepartmentID.ToString() }).ToList();
            return department;
        }
        public IdeaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Year()
        {
            var years = new List<int>();
            for (int i = 2018; i <= DateTime.Now.Year; i++)
            {
                years.Add(i);
            }
            ViewBag.Years = new SelectList(years);
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
        [Authorize(Roles = "Assurance")]
        public async Task<IActionResult> IndexManager(int? pageIndex)
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
        [Authorize(Roles = "Coordinator")]
        public async Task<IActionResult> IndexCoor(int? pageIndex)
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditClosureDate(Idea idea)
        {
            ClosureDateValidation(idea);
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
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public IActionResult Create()
        {
            var un = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(un)).FirstOrDefault();
            ViewBag.image = user.FileName;
            ViewBag.categories = GetDropDownCategory();
            return View();
        }
        [Authorize(Roles = "Staff")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(DocsIdea idea2, List<IFormFile> postedFile)
        {
            IdeaValidation(idea2);
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
        [Authorize(Roles = "Assurance")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Idea idea, List<IFormFile> postedFile)
        {
            IdeaValidation(idea);
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
        [Authorize(Roles = "Coordinator")]
        public IActionResult DetailCoor(int id)
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
        [Authorize(Roles = "Assurance")]
        public IActionResult DetailManager(int id)
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
        [Authorize(Roles = "Staff")]
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
        [Authorize(Roles = "Staff")]
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
        [Authorize(Roles = "Coordinator")]
        public IActionResult MostLikeCoor()
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
        [Authorize(Roles = "Coordinator")]
        public IActionResult MostViewCoor()
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
        [Authorize(Roles = "Assurance")]
        public IActionResult MostLikeManager()
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
        [Authorize(Roles = "Assurance")]
        public IActionResult MostViewManager()
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
        [Authorize(Roles = "Admin")]
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

        public List<Idea> GetIdeaDepartment(int? departmentID, int? year)
        {
            if (year == null && departmentID != null)
            {
                var ideas = (from d in _db.Departments
                             where d.DepartmentID == departmentID
                             join u in _db.CustomUsers on d.DepartmentID equals u.DepartmentID
                             join i in _db.Ideas on u.Id equals i.IdeaUserID
                             select new Idea
                             {
                                 Title = i.Title
                             }).ToList();
                return ideas;
            }
            else if (departmentID != null && year != null)
            {
                var ideas = (from d in _db.Departments
                             where d.DepartmentID == departmentID
                             join u in _db.CustomUsers on d.DepartmentID equals u.DepartmentID
                             join i in _db.Ideas on u.Id equals i.IdeaUserID
                             where i.CreateAt.Year == year
                             select new Idea
                             {
                                 Title = i.Title
                             }).ToList();
                return ideas;
            }
            else if (departmentID == null && year != null)
            {
                var ideas = _db.Ideas.Where(x => x.CreateAt.Year == year).ToList();
                return ideas;
            }
            return null;

        }
        [Authorize(Roles = "Assurance")]
        public IActionResult Dashboard(int? id, string option)
        {
            var username = User.Identity.Name;
            var user = _db.CustomUsers.Where(u => u.UserName.Equals(username)).FirstOrDefault();
            ViewBag.department = ListDepartment();
            ViewBag.image = user.FileName;
            Year();
            return View();
        }
        private void IdeaValidation(Idea idea)
        {
            if (string.IsNullOrEmpty(idea.Title))
            {
                ModelState.AddModelError("Title", "Please Input title");
            }
            if (idea.FirstDate > idea.LastDate)
            {
                ModelState.AddModelError("Date", "You should set FirstDate less than LastDate");
            }

        }
        private void IdeaValidation(DocsIdea idea)
        {
            if (string.IsNullOrEmpty(idea.Title))
            {
                ModelState.AddModelError("Title", "Please Input title");
            }
            if (string.IsNullOrEmpty(idea.Content))
            {
                ModelState.AddModelError("Content", "Please Input content");
            }


        }
        public void ClosureDateValidation(Idea idea)
        {
            if(idea.FirstDate > idea.LastDate)
            {
                ModelState.AddModelError("Date", "You should set FirstDate less than LastDate");
            }
            if (idea.CreateAt > idea.LastDate && idea.CreateAt > idea.FirstDate)
            {
                ModelState.AddModelError("Date", "You should set FirstDate and LastDate more than CreateAt");
            }
        }
    }
}
