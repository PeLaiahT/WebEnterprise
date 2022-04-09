using Microsoft.AspNetCore.Identity;
using System.Data.Entity;
using WebEnterprise.Common;
using WebEnterprise.Data;
using WebEnterprise.Models;
using WebEnterprise.Respon;
namespace WebEnterprise.Respon
{
    public class IdeaRepo : IIdeaRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<CustomUser> userManager;
        public IdeaRepo(ApplicationDbContext db, UserManager<CustomUser> _userManager)
        {
            _db = db;
            userManager = _userManager;

        }

        public IdeaRepo(ApplicationDbContext context)
        {
            _db = context;
        }

        public Idea GetClosureDate(int id)
        {
            var idea = _db.Ideas.Where(i => i.IdeaID == id).FirstOrDefault();
            if (idea != null)
            {
                return idea;
            }
                return null;           
        }

        public Idea GetDetail(int id)
        {
            var idea = _db.Ideas.
               Include(i => i.Category).
               Include(i => i.IdeaUser).
               Include(i => i.Documments).
               FirstOrDefault(i => i.IdeaID == id);
            idea.Comments = _db.Comments.Where(i => i.IdeaID == id).Include(i => i.CommentUser).OrderByDescending(x => x.CreateAt).ToList();
            idea.View++;
            _db.SaveChanges();
            
            if(idea != null)
            {
                return idea;

            }
            return null;
        }

        public Idea GetDetailAdmin(int id)
        {
            var idea = _db.Ideas.
               Include(i => i.Category).
               Include(i => i.IdeaUser).
               Include(i => i.Documments).
               FirstOrDefault(i => i.IdeaID == id);
            idea.Comments = _db.Comments.Where(i => i.IdeaID == id).Include(i => i.CommentUser).OrderByDescending(x => x.CreateAt).ToList();
            if (idea != null)
            {
                return idea;
            }
            return null;
        }

        public Idea GetDetailCoor(int id)
        {
            var idea = _db.Ideas.
                Include(i => i.Category).
                Include(i => i.IdeaUser).
                Include(i => i.Documments).
                FirstOrDefault(i => i.IdeaID == id);
            idea.Comments = _db.Comments.Where(i => i.IdeaID == id).Include(i => i.CommentUser).OrderByDescending(x => x.CreateAt).ToList();
            idea.View++;
            _db.SaveChanges();
            if (idea != null)
            {
                return idea;
            }
            return null;
        }

        public Idea GetDetailManager(int id)
        {
            var idea = _db.Ideas.
               Include(i => i.Category).
               Include(i => i.IdeaUser).
               Include(i => i.Documments).
               FirstOrDefault(i => i.IdeaID == id);
            idea.Comments = _db.Comments.Where(i => i.IdeaID == id).Include(i => i.CommentUser).OrderByDescending(x => x.CreateAt).ToList();
            idea.View++;
            _db.SaveChanges();
            if (idea != null)
            {
                return idea;
            }
            return null;
        }

        //public async Task<IOrderedQueryable> GetIndexIdea(int? page)
        //{
        //    var listIdea = _db.Ideas.Include(i => i.Category)
        //        .Include(i => i.IdeaUser)
        //        .Include(i => i.Documments)
        //        .OrderByDescending(i => i.CreateAt);
        //    if(listIdea != null)
        //    {
        //        return PaginatedList<Idea>.CreateAsync(listIdea, page ?? 1, 5));
        //    }
        //    return null;
        //}

        public Task<IOrderedQueryable> GetIndexIdeaCoor(int page)
        {
            throw new NotImplementedException();
        }

        public Task<IOrderedQueryable> GetIndexIdeaManager(int page)
        {
            throw new NotImplementedException();
        }

        public Task<IOrderedQueryable> GetIndexUser(int page)
        {
            throw new NotImplementedException();
        }

        public Idea GetUpdate(int id)
        {
            var idea = _db.Ideas.Include(i => i.Documments).FirstOrDefault(t => t.IdeaID == id);
            if(idea!= null)
            {
                return idea;
            }
            return null;
        }

        //public Idea MostLike()
        //{
        //    var mostLike = _db.Ideas.Max(i => i.Likecount);
        //    var idea = _db.Ideas.
        //        Include(i => i.IdeaUser).
        //        Include(i => i.Category).
        //        Include(i => i.Documments).
        //        Include(i => i.Comments).ThenInclude(c => c.CommentUser)
        //        .Where(i => i.Likecount == mostLike).FirstOrDefault();
        //    if(idea != null)
        //    {
        //        return idea;
        //    }
        //    return null;
        //}

        public Idea MostLikeCoor()
        {
            throw new NotImplementedException();
        }

        public Idea MostLikeManager()
        {
            throw new NotImplementedException();
        }

        public Idea MostLikeStaff()
        {
            throw new NotImplementedException();
        }

        public Idea MostView()
        {
            throw new NotImplementedException();
        }

        public Idea MostViewCỏor()
        {
            throw new NotImplementedException();
        }

        public Idea MostViewManager()
        {
            throw new NotImplementedException();
        }

        public Idea MostViewStaff()
        {
            throw new NotImplementedException();
        }

        public Idea PostClosureDate(Idea idea)
        {
            throw new NotImplementedException();
        }

        public Task<Idea> PostCreate(Idea idea, List<IFormFile> postedFile)
        {
            throw new NotImplementedException();
        }

        public Task<Idea> PostUpdate(Idea idea, List<IFormFile> postedFile)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        bool IIdeaRepo.DeleteDoc(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IOrderedQueryable> GetIndexIdea(int page)
        {
            throw new NotImplementedException();
        }

        public Idea MostLike()
        {
            throw new NotImplementedException();
        }
    }
}
