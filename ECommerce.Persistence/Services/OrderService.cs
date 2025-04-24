using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities.Enums;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Payments;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<OrderDto> CreateOrderAsync(string userId, CreateOrderDto createOrderDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var productIds = createOrderDto.Items.Select(item => item.ProductId).ToList();
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();
                if (products.Count != productIds.Count)
                {
                    throw new InvalidOperationException("Some of the required products are not present");
                }

                decimal totalAmount = 0;
                var orderItems = new List<OrderItem>();

                foreach (var item in createOrderDto.Items)
                {
                    var product = products.First(p => p.Id == item.ProductId);
                    if (product.StockQuantity < item.Quantity)
                    {
                        throw new InvalidOperationException($"Not enough stock for product {product.Name}");
                    }

                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                    };
                    orderItems.Add(orderItem);
                    totalAmount += orderItem.Quantity * orderItem.UnitPrice;
                    product.StockQuantity -= item.Quantity;
                }

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = totalAmount,
                    Status = OrderStatus.Pending,
                    OrderItems = orderItems,
                    Payment = new Payment
                    {
                        Amount = totalAmount,
                        PaymentMethod = createOrderDto.Payment.PaymentMethod,
                        PaymentDate = DateTime.UtcNow,
                    },
                    Shipping = new Shipping
                    {
                        TrackingNumber = GenerateTrackingNumber(),
                        ShippingDate = DateTime.UtcNow,
                        Status = "Pending",
                    }
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var createdOrder = await GetOrderByIdAsync(order.Id, userId);
                return createdOrder;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
            

        public async Task<OrderDto> GetOrderByIdAsync(int orderId, string userId)
        {
            var order= await _context.Orders
                .Include(o=>o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Payment)
                .Include(o => o.Shipping)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order == null)
                return null!;
            return MapOrderToDto(order);
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Payment)
                .Include(o => o.Shipping)
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return orders.Select(MapOrderToDto).ToList();
        }
        public async Task<IEnumerable<OrderDto>> GetSellerOrdersAsync(string sellerId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(oi => oi.ProductBrand)
                .Where(o => o.OrderItems.Any(oi => oi.Product.ProductBrand.sellerId == sellerId))
                .Include(o => o.Payment)
                .Include(o => o.Shipping)
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return orders.Select(MapOrderToDto).ToList();
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateStatusDto, string userId, bool isAdminOrSeller)
        {
            if (!isAdminOrSeller)
            {
                throw new UnauthorizedAccessException("You do not have the authority to update the request status");
            }
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Payment)
                .Include(o => o.Shipping)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return null;
            }

            order.Status = updateStatusDto.Status;

            if (updateStatusDto.Status == OrderStatus.Shipped)
            {
                order.Shipping.Status = "Shipped";
                order.Shipping.ShippingDate = DateTime.UtcNow;
            }

            else if (updateStatusDto.Status == OrderStatus.Delivered)
            {
                order.Shipping.Status = "Delivered";
            }
            else if (updateStatusDto.Status == OrderStatus.Cancelled)
            {
                order.Shipping.Status = "Cancelled";

                foreach (var item in order.OrderItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity += item.Quantity;
                    }
                }
            }
            await _context.SaveChangesAsync();
            return MapOrderToDto(order);
        }

        private OrderDto MapOrderToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                UserId = order.UserId,
                UserName = order.User.UserName!,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    ProductImageUrl= oi.Product.ImageUrl,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.Quantity * oi.UnitPrice
                }).ToList(),
                Payment = order.Payment == null ? null : new PaymentDto
                {
                    Id = order.Payment.Id,
                    PaymentMethod = order.Payment.PaymentMethod,
                    PaymentDate = order.Payment.PaymentDate,
                    Amount = order.Payment.Amount,
                    OrderId = order.Id,
                    Status = order.Status == OrderStatus.Refunded ? "Refunded" : "Completed",
                },
                Shipping = order.Shipping == null ? null : new ShippingDto
                {
                    Id = order.Shipping.Id,
                    TrackingNumber = order.Shipping.TrackingNumber,
                    ShippingDate = order.Shipping.ShippingDate,
                    Status = order.Shipping.Status,
                }
            };
            
        } 

        private bool IsAdminOrSeller(string userId)
        {
           var user=_context.Users.FirstOrDefault(u => u.Id == userId);
            
            return true;
        }

        private string GenerateTrackingNumber()
        {
            return $"TRK-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}
