using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase.Entities
{
    internal class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        
        
        public int SourceId { get; set; }
        public Source Source { get; set; }

        public List<Comment?> Comments { get; set; } 
    }
}
