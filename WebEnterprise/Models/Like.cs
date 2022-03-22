using System.ComponentModel.DataAnnotations;
using WebEnterprise.Data;

namespace WebEnterprise.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        public int IdeaId { get;set; }
        public Idea? Idea { get; set; }
        public string? LikeUserID { get; set; }
        public CustomUser? LikeUser { get; set; }
    }
}
