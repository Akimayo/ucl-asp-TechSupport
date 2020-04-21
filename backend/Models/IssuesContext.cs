using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechSupport.Backend.Models.Entities;

namespace TechSupport.Backend.Models
{
    public class IssuesContext : DbContext
    {
        public IssuesContext (DbContextOptions<IssuesContext> options)
            : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Report>().ToTable("Report");
            modelBuilder.Entity<Resolution>().ToTable("Resolution");
        }
    }
}
