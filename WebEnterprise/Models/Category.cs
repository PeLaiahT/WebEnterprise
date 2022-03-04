using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebEnterprise.Models
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        public string NameCategory { get; set; }
        [Required(ErrorMessage = "Please enter Description")]
        public string Desciption { get; set; }
        public List<Idea> Ideas { get; set; }
        
    }
}
