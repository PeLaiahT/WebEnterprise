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

        public Idea UpLike(int IdeaID)
        {
            throw new NotImplementedException();
        }
    }
}
