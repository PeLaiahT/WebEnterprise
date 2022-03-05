using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.Models
{
    public class CustomUser: IdentityUser
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Image")]
        public string file { get; set; }
    }
}
