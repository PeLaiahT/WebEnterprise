using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebEnterprise.Models
{
    public class CustomUser: IdentityUser
    {
        public string? FullName { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
