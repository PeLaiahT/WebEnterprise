using WebEnterprise.Models;

namespace WebEnterprise.Respon
{
    public interface ICategoryRepo
    {
        public List<Category> GetListCategory();
        public Category PostCreate(Category category);
        public bool DeleteCategory(int id);
        public Category GetUpdate(int id);
        public Category PostUpdate(Category category);
    }
}
