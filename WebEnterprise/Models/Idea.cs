using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.Models
{
    public class Idea
    {
        [Key]
        public int IdeaID { get; set; }
        [Required(ErrorMessage = "Please enter Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter Content")]
        public string Content { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime FirstDate { get; set; } = DateTime.Now;
        public DateTime LastDate { get; set; } = DateTime.Now;
        public int Like { get; set; } = 0;
        public int View { get; set; } = 0;
        public List<Comment> Comments { get; set; }

    }
}
