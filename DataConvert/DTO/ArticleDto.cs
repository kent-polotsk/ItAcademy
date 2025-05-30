﻿using EFDatabase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConvert.DTO
{
    public class ArticleDto
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
        public bool IsSent { get; set; }

        //public int SourceId { get; set; }
        public string SourceName { get; set; }

        //public List<Comment?> Comments { get; set; }
    }
}
