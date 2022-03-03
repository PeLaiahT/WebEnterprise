using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.Models
{
    public class Comment
    {
        [Key]
        public int IdCommment { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public int Like { get; set; } = 0;

        [Required(ErrorMessage = "Please enter Content")]
        public string Content { get; set; }
        public int IdeaId { get; set; }
        public Idea Idea { get; set; }
    }
}
