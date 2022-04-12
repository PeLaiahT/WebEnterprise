using System.Data.Entity;
using WebEnterprise.Data;
using WebEnterprise.Models;

namespace WebEnterprise.Respon
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool DeleteCategory(int id)
        {
            var courseCategory = _db.Categories.FirstOrDefault(t => t.CategoryID == id);
            if (courseCategory != null)
            {
                _db.Categories.Remove(courseCategory);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Category> GetListCategory()
        {
            var categories = _db.Categories
                              .OrderBy(c => c.CategoryID)
                              .ToList();
            if(categories != null)
            {
                return categories;
            }
            return null;
        }

        public Category GetUpdate(int id)
        {
            var category = _db.Categories.FirstOrDefault(t => t.CategoryID == id);
            if (category != null)
            {
                return category;
            }
            else
            {
                return null;
            }
        }

        public Category PostCreate(Category category)
        {
            
            if(category!=null)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                return category;
            }
            return null;
        }

        public Category PostUpdate(Category category)
        {
            if(category!=null)
            {

                _db.Update(category);
                _db.SaveChanges();
                return category;
            }
            return null;
        }
    }
}