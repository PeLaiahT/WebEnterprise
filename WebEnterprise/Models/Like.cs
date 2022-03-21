using System.ComponentModel.DataAnnotations;
using WebEnterprise.Data;

namespace WebEnterprise.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }
        public int IdeaID { get;set; }
        public Idea Idea { get; set; }
        public string UserID { get; set; }
        public CustomUser User { get; set; }
    }
}
