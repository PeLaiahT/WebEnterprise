using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebEnterprise.Data;
using WebEnterprise.Models;
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
            if(idea != null)
            {
                _db.Update(idea);
                _db.SaveChanges();
                return idea;
            }
            return null;
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
            var idea = _db.Ideas.FirstOrDefault(t => t.IdeaID == id);
            if (idea != null)
            {
                _db.Ideas.Remove(idea);
                _db.SaveChanges();
                return true;
            }
            return false;

        }

        public bool DeleteDoc(int id)
        {
            var doc = _db.Documments.FirstOrDefault(t => t.DocummentID == id);
            if (doc != null)
            {
                _db.Documments.Remove(doc);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public Task<IOrderedQueryable> GetIndexIdea(int page)
        {
            throw new NotImplementedException();
        }

        public Idea MostLike()
        {
            throw new NotImplementedException();

        }

        public Idea PostCreate(Idea idea)
        {
            if(idea != null)
            {
                var idea1 = new Idea
                {
                    Content = idea.Content,
                    Title = idea.Title,
                    CategoryID = idea.CategoryID,
                };
                _db.Ideas.Add(idea1);
                _db.SaveChanges();
                return idea;
            }
            return null;
        }

        public Idea PostUpdate(Idea idea)
        {
            if (idea != null)
            {
                _db.Update(idea);
                _db.SaveChanges();
                return idea;
            }
            return null;
        }
    }
}
