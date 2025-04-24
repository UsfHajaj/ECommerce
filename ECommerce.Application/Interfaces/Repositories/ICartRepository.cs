using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Repositories
{
    public interface ICartRepository :IGenericRepository<Cart>
    {
        Task<Cart> GetCartWithItemsByUserIdAsync(string userId);
        Task<CartItem> GetCartItemByIdAsync(int id);
    }
}
