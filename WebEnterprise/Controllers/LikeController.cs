using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebEnterprise.Data;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class LikeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LikeController(ApplicationDbContext db)
        {
            _db = db;
        }
        [Authorize]
        [HttpPost]
        public IActionResult UpLike(int IdeaID)
        {
            var idea = _db.Ideas.FirstOrDefault(i => i.IdeaID == IdeaID);
            if (idea != null)
            {
                var like = new Like
                {
                    UserID = User.Identity.GetUserId(),
                    IdeaID = IdeaID
                };
                _db.Likes.Add(like);
                idea.Likecount++;
                _db.SaveChanges();
            }
            return RedirectToAction("Detail", "Idea", new { Id = IdeaID });
        }
        [Authorize]
        [HttpPost]
        public IActionResult DownLike(int IdeaID)
        {
            var idea = _db.Ideas.FirstOrDefault(i => i.IdeaID == IdeaID);
            var like = _db.Likes.Where(l => l.IdeaID == IdeaID).FirstOrDefault();
            if(like != null && idea != null)
            {
                _db.Likes.Remove(like);
                idea.Likecount--;
                _db.SaveChanges();
                return RedirectToAction("Detail", "Idea", new { Id = IdeaID });
            }
            else
            {
                idea.Likecount = 0;
            }
            return RedirectToAction("Detail", "Idea", new { Id = IdeaID });
        }

    }
}
