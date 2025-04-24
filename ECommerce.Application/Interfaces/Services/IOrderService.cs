using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string userId, CreateOrderDto createOrderDto);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId);
        Task<OrderDto> GetOrderByIdAsync(int orderId, string userId);
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto updateStatusDto, string userId, bool isAdminOrSeller);
        Task<IEnumerable<OrderDto>> GetSellerOrdersAsync(string sellerId);
    }
}
