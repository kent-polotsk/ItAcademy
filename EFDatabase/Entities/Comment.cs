using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
