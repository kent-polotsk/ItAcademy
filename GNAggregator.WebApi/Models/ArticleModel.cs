namespace GNAggregator.WebApi.Models
{
    public class ArticleModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Url { get; set; }
        public string? ImageUrl { get; set; }

        public double? PositivityRate { get; set; }

        public int SourceId { get; set; }
        public string SourceName { get; set; }

        //public List<Comment?> Comments { get; set; }
    }
}
