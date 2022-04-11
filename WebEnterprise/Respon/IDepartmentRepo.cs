using WebEnterprise.Models;

namespace WebEnterprise.Respon
{
    public interface IDepartmentRepo
    {
        public List<Department> GetListCategory();
        public Department PostCreate(Department department);
        public bool DeleteDepartment(int id);
        public Department GetUpdate(int id);
    }
}
