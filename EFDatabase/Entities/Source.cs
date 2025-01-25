using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDatabase.Entities
{
    internal class Source
    {
        public int Id { get; set; } 
        public string? Name { get; set; }
        public string URL { get; set; }

        public List<Article?> Articles { get; set; }
    }
}
