using System.ComponentModel.DataAnnotations;

namespace WebEnterprise.Models
{
    public class Documment
    {
        [Key]
        public int DocummentID { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long? FileSize { get; set; }
        public Idea Idea { get; set; }
        public int IdeaID { get; set; }

    }
}
