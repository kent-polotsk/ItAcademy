using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace EFDatabase.Entities
{
    internal class GNAggregatorContext : DbContext
    {
        DbSet<Article> Articles { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<Source> Sources { get; set; }
        DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=Intel;Database=GNAggregator;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
