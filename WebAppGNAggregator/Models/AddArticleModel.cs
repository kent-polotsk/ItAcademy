using System.ComponentModel.DataAnnotations;

namespace WebAppGNAggregator.Models
{
    public class AddArticleModel
    {
        //public Guid Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        [Required]
        [MinLength(10)]
        public string Description { get; set; }
        
        //public DateTime CreationDate { get; set; }
        

        [Range(1,10)]
        public double? Rate { get; set; }

    }
}
