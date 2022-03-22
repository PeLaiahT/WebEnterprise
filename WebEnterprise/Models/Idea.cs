using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.Models
{
    public class Idea
    {
        [Key]
        public int IdeaID { get; set; }
        [Required(ErrorMessage = "Please enter Title")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Please enter Content")]
        public string? Content { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime FirstDate { get; set; } = DateTime.Now;
        public DateTime LastDate { get; set; } = DateTime.Now;
        public int View { get; set; } = 0;

        public List<Comment>? Comments { get; set; }
        public int? CategoryID { get; set; }
        public Category? Category { get; set; }
        public List<Documment>? Documments { get; set; }
        public List<Like>? Likes { get; set; }
        public int Likecount { get; set; } = 0;
        public string? IdeaUserID { get; set; }
        public CustomUser? IdeaUser { get; set; }

    }
}
