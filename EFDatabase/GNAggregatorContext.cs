using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFDatabase.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFDatabase
{
    public class GNAggregatorContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<User> Users { get; set; }

        public GNAggregatorContext() { }
        public GNAggregatorContext(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=Intel;Database=GNAggregator;Trusted_Connection=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasMany(p=>p.Comments)
                .WithOne(c=>c.Article)
                .HasForeignKey(c=>c.ArticleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Source>()
                .HasMany(p => p.Articles)
                .WithOne(c => c.Source)
                .HasForeignKey(c => c.SourceId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            
        }
    }
}
