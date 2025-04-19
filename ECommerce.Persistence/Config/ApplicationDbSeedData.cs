using ECommerce.Domain.Entities.Enums;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Payments;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Config
{
    public static class ApplicationDbSeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
            {
                new Category { Name = "Electronics", Description = "Electronic gadgets" },
                new Category { Name = "Books", Description = "Books and novels" },
                new Category { Name = "Clothing", Description = "Men and Women clothing" }
            };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            if (!context.ProductBrands.Any())
            {
                var brands = new List<ProductBrand>
            {
                new ProductBrand { Name = "Apple" },
                new ProductBrand { Name = "Samsung" },
                new ProductBrand { Name = "Nike" }
            };
                context.ProductBrands.AddRange(brands);
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var products = new List<Product>
            {
                new Product { Name = "iPhone 14", Description = "Latest iPhone", Price = 1000, StockQuantity = 10, ImageUrl = "iphone.jpg", CategoryId = 1, ProductBrandId = 1 },
                new Product { Name = "Samsung Galaxy", Description = "New Samsung Phone", Price = 900, StockQuantity = 15, ImageUrl = "samsung.jpg", CategoryId = 1, ProductBrandId = 2 },
                new Product { Name = "Nike T-Shirt", Description = "Comfortable shirt", Price = 40, StockQuantity = 50, ImageUrl = "nike.jpg", CategoryId = 3, ProductBrandId = 3 }
            };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

            //if (!context.Reviews.Any())
            //{
            //    var reviews = new List<Review>
            //{
            //    new Review { ProductId = 1, UserId = "user1", Comment = "Amazing!", Rating = 5 },
            //    new Review { ProductId = 2, UserId = "user2", Comment = "Pretty good", Rating = 4 },
            //    new Review { ProductId = 3, UserId = "user3", Comment = "Good value", Rating = 4 }
            //};
            //    context.Reviews.AddRange(reviews);
            //    await context.SaveChangesAsync();
            //}

            //if (!context.Orders.Any())
            //{
            //    var orders = new List<Order>
            //{
            //    new Order
            //    {
            //        UserId = "user1",
            //        OrderDate = DateTime.UtcNow.AddDays(-3),
            //        TotalAmount = 1000,
            //        Status = OrderStatus.PaymentRecieved,
            //        OrderItems = new List<OrderItem>
            //        {
            //            new OrderItem { ProductId = 1, Quantity = 1, UnitPrice = 1000 }
            //        },
            //        Payment = new Payment { PaymentMethod = "Visa", PaymentDate = DateTime.UtcNow.AddDays(-2), Amount = 1000 },
            //        Shipping = new Shipping { ShippingDate = DateTime.UtcNow.AddDays(-1), Status = "Shipped", TrackingNumber = "TRK123" }
            //    }
            //};

            //    context.Orders.AddRange(orders);
            //    await context.SaveChangesAsync();
            //}
        }

    }
}
