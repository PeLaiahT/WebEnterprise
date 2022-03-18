using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebEnterprise.Models
{
    public class CustomUser: IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public byte[]? Image { get; set; }
        public string? FileName { get; set; } 
        public int? DepartmentID { get; set; }
        public Department? Department { get; set; }
    }
}
