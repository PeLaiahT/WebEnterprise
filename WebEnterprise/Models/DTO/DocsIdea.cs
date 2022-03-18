namespace WebEnterprise.Models.DTO
{
    public class DocsIdea
    {
        public int? DocummentID { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public string? Content { get; set; }
        public int? View { get; set; } = 0;
        public List<Comment>? Comments { get; set; }
        public int? CategoryID { get; set; }
        public Category? Category { get; set; }
        public List<Documment>? Documments { get; set; }
        public Idea? Idea { get; set; }
        public int? IdeaID { get; set; }
        public string? CategoryName { get; set; }
        public string? Title { get; set; }

        public long? FileSize { get; set; }
        public DocsIdea()
        {
            Documments = new List<Documment>();
        }
    }
}
