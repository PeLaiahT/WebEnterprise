using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebEnterprise.Models
{
    public class CustomUser: IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }

        public string ImageName { get; set; }
        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile ProfileImage { get; set; }
    }
}
