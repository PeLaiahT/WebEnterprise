using WebEnterprise.Data;
using WebEnterprise.Models;

namespace WebEnterprise.Respon
{
    public class DepartmentRepo : IDepartmentRepo
    {
        private readonly ApplicationDbContext _db;
        public DepartmentRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool DeleteDepartment(int id)
        {
            var department = _db.Departments.FirstOrDefault(t => t.DepartmentID == id);
            if(department!= null)
            {
                _db.Departments.Remove(department);
                _db.SaveChanges();
                return true;
            }    
            return false;
        }

        public List<Department> GetListCategory()
        {
            var departments = _db.Departments
                          .OrderBy(c => c.DepartmentID)
                          .ToList();
            if(departments != null)
            {
                return departments;
            }
            return null;
        }


        public Department PostCreate(Department department)
        {
          
            if (department!=null)
            {
                _db.Departments.Add(department);
                _db.SaveChanges();
                return department;
            }
            return null;
        }

        public Department GetUpdate(int id)
        {
            var department = _db.Departments.FirstOrDefault(t => t.DepartmentID == id);
            if (department != null)
            {
                return department;

            }
            return null;
        }
    }
}
