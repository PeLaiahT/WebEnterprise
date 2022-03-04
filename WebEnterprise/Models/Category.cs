
using System.ComponentModel.DataAnnotations;


namespace WebEnterprise.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        [Required(ErrorMessage = "Please enter Name")]
        public string NameCategory { get; set; }
        [Required(ErrorMessage = "Please enter Description")]
        public string Desciption { get; set; }
        public List<Idea> Ideas { get; set; }
        
    }
}
