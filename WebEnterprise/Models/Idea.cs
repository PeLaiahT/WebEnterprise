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
        public DateTime CreateAt { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public int Like { get; set; }
        public int View { get; set; }
        public Idea()
        {
            CreateAt = DateTime.Now;
            Like = 0;
            View = 0;
        }

    }
}
