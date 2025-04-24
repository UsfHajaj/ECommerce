using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities.Enums;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _dbContext;

        public PaymentService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<PaymentDto> GetPaymentByIdAsync(int paymentId, string userId)
        {
            var payment = await _dbContext.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p=>p.Id==paymentId&&p.Order.UserId==userId);
            if (payment == null)
                return null;
            return new PaymentDto
            {
                Id = payment.Id,
                PaymentMethod = payment.PaymentMethod,
                PaymentDate = payment.PaymentDate,
                Amount = payment.Amount,
                OrderId = payment.OrderId,
                Status = payment.Order.Status == OrderStatus.Refunded ? "Refunded" : "Completed"
            };
        }

        public async Task<PaymentDto> ProcessPaymentAsync(string userId, PaymentProcessDto paymentProcessDto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var order = await _dbContext.Orders
                    .Include(o => o.Payment)
                    .FirstOrDefaultAsync(o => o.Id == paymentProcessDto.OrderId && o.UserId == userId);
                if (order == null)
                {
                    throw new InvalidOperationException("Order not found.");
                }
                if (order.Payment != null)
                {
                    throw new InvalidOperationException("Payment already processed for this order.");
                }
                var payment = new Domain.Entities.Payments.Payment
                {
                    OrderId = order.Id,
                    PaymentMethod = paymentProcessDto.PaymentMethod,
                    PaymentDate = DateTime.UtcNow,
                    Amount = order.TotalAmount
                };
                await _dbContext.Payments.AddAsync(payment);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new PaymentDto
                {
                    Id = payment.Id,
                    PaymentMethod = payment.PaymentMethod,
                    PaymentDate = payment.PaymentDate,
                    Amount = payment.Amount,
                    OrderId = payment.OrderId,
                    Status = "Completed"
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PaymentDto> RefundPaymentAsync(int paymentId, PaymentRefundDto refundDto, string userId)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var payment = await _dbContext.Payments
                    .Include(p => p.Order)
                    .ThenInclude(o => o.OrderItems)
                    .FirstOrDefaultAsync(p => p.Id == paymentId && p.Order.UserId == userId);
                if (payment == null)
                {
                    return null;
                }
                if (refundDto.Amount>payment.Amount)
                {
                    throw new InvalidOperationException("The recovery amount is greater than the original payment amount");
                }
                payment.Order.Status = OrderStatus.Refunded;
                if(refundDto.Amount==payment.Amount)
                {
                    foreach(var i in payment.Order.OrderItems)
                    {
                        var product = await _dbContext.Products.FindAsync(i.ProductId);
                        if (product != null)
                        {
                            product.StockQuantity += i.Quantity;
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new PaymentDto
                {
                    Id = payment.Id,
                    PaymentMethod = payment.PaymentMethod,
                    PaymentDate = payment.PaymentDate,
                    Amount = payment.Amount,
                    OrderId = payment.OrderId,
                    Status = "Refunded"
                };

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
