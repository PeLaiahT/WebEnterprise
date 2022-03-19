
using System.ComponentModel.DataAnnotations;


namespace WebEnterprise.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public int Like { get; set; } = 0;

        [Required(ErrorMessage = "Please enter Content")]
        public string Content { get; set; }
        public int IdeaID { get; set; }
        public Idea Idea { get; set; }
        public string UserID { get; set; }
        public CustomUser User { get; set; }


    }
}
