using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CommentController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Authorize(Roles ="Staff")]
        [HttpPost]
        public IActionResult Create(string NoiDung, int IdeaID)
        {
            CommentValidation(NoiDung);
            var comment = new Comment
            {
                Content = NoiDung,
                IdeaID = IdeaID,
                CommentUserID = User.Identity.GetUserId(),
            };
            _db.Comments.Add(comment);
            _db.SaveChanges();
            return RedirectToAction("Detail", "Idea", new {Id = IdeaID});
        }
        private void CommentValidation(string cmt)
        {
            if (string.IsNullOrEmpty(cmt))
            {
                ModelState.AddModelError("Content", "Please Input Content");
            }    
        }

        }
}
