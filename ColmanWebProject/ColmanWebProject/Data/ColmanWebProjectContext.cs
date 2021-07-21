using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ColmanWebProject.Models;

namespace ColmanWebProject.Data
{
    public class ColmanWebProjectContext : DbContext
    {
        public ColmanWebProjectContext (DbContextOptions<ColmanWebProjectContext> options)
            : base(options)
        {
        }

        public DbSet<ColmanWebProject.Models.Customer> Customer { get; set; }
        public DbSet<ColmanWebProject.Models.Product> Product { get; set; }

        public DbSet<ColmanWebProject.Models.Category> Category { get; set; }

        public DbSet<ColmanWebProject.Models.Cart> Cart { get; set; }

        public DbSet<ColmanWebProject.Models.WishList> WishList { get; set; }
        public DbSet<ColmanWebProject.Models.ProductsWishList> ProductsWishList { get; set; }
        public DbSet<ColmanWebProject.Models.ProductsCart> ProductsCart { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductsWishList>()
                .HasKey(cs => new { cs.ProductId, cs.WishListId });
            modelBuilder.Entity<ProductsCart>()
                .HasKey(cs => new { cs.ProductId, cs.CartId });
        }
    }
}
