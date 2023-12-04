using Microsoft.EntityFrameworkCore;
using ServiceZeuss.Data.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ServiceZeuss.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Category> tblCategory { get; set; }
        public DbSet<Product> tblProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.StrName).IsUnique();
        }
    }
}
