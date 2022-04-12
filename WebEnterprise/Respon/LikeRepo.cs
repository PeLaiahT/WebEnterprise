using WebEnterprise.Data;
using WebEnterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.Identity;
namespace WebEnterprise.Respon
{
    public class LikeRepo : ILikeRepo
    {
        private readonly ApplicationDbContext _db;
        public LikeRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public Idea DownLike(int IdeaId)
        {
            throw new NotImplementedException();
        }
      
        public Idea UpLike(int IdeaId)
        {
            var idea = _db.Ideas.FirstOrDefault(i => i.IdeaID == IdeaId);
            if (idea != null)
            {
                var like = new Like
                {
                    //LikeUserID = User.Identity.GetUserId(),
                    IdeaId = IdeaId
                };
                _db.Likes.Add(like);
                idea.Likecount++;
                _db.SaveChanges();
                return idea;
            }
            return null;
        }
    }
}
