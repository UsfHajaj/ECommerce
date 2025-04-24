using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface ICartServices
    {
        Task<CartDto> GetCartAsync(string userId);
        Task<CartItemDto> AddItemToCartAsync(string userId, AddCartItemDto addCartItemDto);
        Task<CartItemDto> UpdateCartItemAsync(string userId, UpdateCartItemDto updateCartItemDto);
        Task<bool> RemoveCartItemAsync(string userId, int cartItemId);
    }
}
