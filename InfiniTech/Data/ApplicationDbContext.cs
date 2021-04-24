using Domain;
using Domain.Products;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfiniTech.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OrderedProducts>().HasKey(t => new {t.OrderId,t.ProductId });
            builder.Entity<UserLikesProduct>().HasKey(t => new {t.ApplicationUserId,t.ProductId });
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<UserLikesProduct> UserLikedProducts { get; set; }
        public DbSet<OrderedProducts> OrderedProducts { get; set; }
        public DbSet<Laptop> Laptops{ get; set; }
        public DbSet<Smartphone> Smartphones { get; set; }
    }
}
