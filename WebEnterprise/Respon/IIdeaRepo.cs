using WebEnterprise.Models;

namespace WebEnterprise.Respon
{
    public interface IIdeaRepo
    {
        public Task<IOrderedQueryable> GetIndexIdea(int page);
        public Task<IOrderedQueryable> GetIndexIdeaManager(int page);
        public Task<IOrderedQueryable> GetIndexIdeaCoor(int page);
        public Task<IOrderedQueryable> GetIndexUser(int page);
        public Idea GetClosureDate(int id);
        public Idea PostClosureDate(Idea idea);

        public bool Delete(int id);
        public Idea GetUpdate(int id);
        public Idea GetDetail(int id);
        public Idea GetDetailCoor(int id);
        public Idea GetDetailManager(int id);
        public Idea GetDetailAdmin(int id);
        public Idea MostLike();
        public Idea MostView();
        public Idea MostLikeStaff();
        public Idea MostViewStaff();
  
        public Idea MostLikeCoor();
        public Idea MostViewCỏor();
        public Idea MostLikeManager();
        public Idea MostViewManager();
        public bool DeleteDoc(int id);
        public Task<Idea> PostCreate(Idea idea, List<IFormFile> postedFile);
        public Task<Idea> PostUpdate(Idea idea, List<IFormFile> postedFile);
        public Idea PostCreate(Idea idea);
        public Idea PostUpdate(Idea idea);








    }
}
