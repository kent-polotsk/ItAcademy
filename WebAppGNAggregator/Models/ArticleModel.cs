using System.ComponentModel.DataAnnotations;

namespace WebAppGNAggregator.Models
{
    public class AddArticleModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        
        public double? Rate { get; set; }
            
    }
}
