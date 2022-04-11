using WebEnterprise.Models;

namespace WebEnterprise.Respon
{
    public interface ILikeRepo
    {
        public Idea UpLike(int IdeaID);
        public Idea DownLike(int IdeaId);
    }
}
