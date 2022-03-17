using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        public string NameDepartment { get; set; }
        public string? Description { get; set; }
        public List<CustomUser> CustomUsers { get; set; }

    }
}
