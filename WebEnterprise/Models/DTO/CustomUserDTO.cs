



namespace WebEnterprise.Models.DTO
{
    public class CustomUserDTO
    {
        public string? Id { get; set; }
        public string UserName { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[]? Image { get; set; }
        public string? FileName { get; set; }
        public Department? Department { get; set; }
        public int DepartmentID { get; set; }
    }
}
