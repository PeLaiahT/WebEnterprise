namespace WebEnterprise.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public string NameDepartment { get; set; }
        public string? Description { get; set; }
        public List<CustomUser> CustomUsers { get; set; }

    }
}
