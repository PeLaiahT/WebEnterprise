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
        [HttpPost]
        public IActionResult UpLike(int IdeaID)
        {
            var like = new Like
            {
                IdeaID = IdeaID
            };
            _db.Likes.Add(like);
            _db.SaveChanges();
            return RedirectToAction("Detail", "Idea", new { Id = IdeaID });
        }
        [HttpPost]
        public IActionResult DownLike(int IdeaID)
        {
            var like = _db.Likes.Where(l => l.IdeaID == IdeaID).FirstOrDefault();
            if(like != null)
            {
                _db.Likes.Remove(like);
                _db.SaveChanges();
                return RedirectToAction("Detail", "Idea", new { Id = IdeaID });
            }
            return RedirectToAction("Index", "Idea");
        }

    }
}
