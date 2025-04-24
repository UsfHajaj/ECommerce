using ECommerce.Domain.Entities.Cart;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Payments;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace ECommerce.Persistence.Contexts
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>()
                        .HasOne(p => p.ProductBrand) 
                        .WithMany(s => s.Products) 
                        .HasForeignKey(p => p.ProductBrandId) 
                        .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Product>()
                        .HasOne(p => p.Category)
                        .WithMany(c => c.Products) 
                        .HasForeignKey(p => p.CategoryId) 
                        .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CartItem>()
                        .HasOne(ci => ci.Cart) 
                        .WithMany(c => c.CartItems) 
                        .HasForeignKey(ci => ci.cartId) 
                        .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<OrderItem>()
                        .HasOne(oi => oi.Order) 
                        .WithMany(o => o.OrderItems) 
                        .HasForeignKey(oi => oi.OrderId) 
                        .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Payment>()
                        .HasOne(p => p.Order) 
                        .WithOne(o => o.Payment) 
                        .HasForeignKey<Payment>(p => p.OrderId) 
                        .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<Shipping>()
                        .HasOne(s => s.Order) 
                        .WithOne(o => o.Shipping) 
                        .HasForeignKey<Shipping>(s => s.OrderId)
                        .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Review>()
                        .HasOne(r => r.Product) 
                        .WithMany(p => p.Reviews) 
                        .HasForeignKey(r => r.ProductId) 
                        .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Review>()
                        .HasOne(r => r.User)
                        .WithMany(p => p.Reviews)
                        .HasForeignKey(r => r.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>()
                        .HasOne(a => a.Address)
                        .WithOne(b => b.ApplicationUser)
                        .HasForeignKey<Address>(b => b.UserId);
            builder.Entity<Product>()
                        .Property(p => p.Price)
                        .HasPrecision(18, 2);
            builder.Entity<OrderItem>()
                        .Property(o => o.UnitPrice)
                        .HasPrecision(18, 2);

            builder.Entity<Order>()
                        .Property(o => o.TotalAmount)
                        .HasPrecision(18, 2);

            builder.Entity<Payment>()
                        .Property(p => p.Amount)
                        .HasPrecision(18, 2);

            builder.Entity<CartItem>()
                        .Property(c => c.Price)
                        .HasPrecision(18, 2);
            builder.Entity<Sellers>()
                        .HasOne(m => m.ProductBrand)
                        .WithOne(m => m.Sellers)
                        .HasForeignKey<ProductBrand>(m => m.sellerId)
                        .IsRequired(false);
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Shipping> Shippings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<Sellers> Sellers { get; set; }

    }
}
