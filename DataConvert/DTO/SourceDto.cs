using EFDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConvert.DTO
{
    public class SourceDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string URL { get; set; }
        public string RSSURL { get; set; }

    }
}
