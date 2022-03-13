using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebEnterprise.Models
{
    public class FileDetails
    {
        [Key]
        public int FileDetailID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
    public class FileModel
    {
        [Key]
        public int FileID { get; set; }
        public int IdeaID { get; set; }
        public Idea Idea { get; set; }
        public List<FileDetails> Files { get; set; }
    = new List<FileDetails>();
    }
}
