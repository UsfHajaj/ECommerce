using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> ProcessPaymentAsync(string userId, PaymentProcessDto paymentProcessDto);
        Task<PaymentDto> GetPaymentByIdAsync(int paymentId, string userId);
        Task<PaymentDto> RefundPaymentAsync(int paymentId, PaymentRefundDto refundDto, string userId);
    }
}
