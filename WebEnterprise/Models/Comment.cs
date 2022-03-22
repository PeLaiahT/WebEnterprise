
using System.ComponentModel.DataAnnotations;


namespace WebEnterprise.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter Content")]
        public string Content { get; set; }
        public int IdeaID { get; set; }
        public Idea? Idea { get; set; }
        public string? CommentUserID { get; set; }
        public CustomUser? CommentUser { get; set; }


    }
}
