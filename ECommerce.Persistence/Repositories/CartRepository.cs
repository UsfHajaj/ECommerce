using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Domain.Entities.Cart;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class CartRepository : GenericRepository<Cart> ,ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CartItem> GetCartItemByIdAsync(int id)
        {
            return await _context.CartItems
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Cart> GetCartWithItemsByUserIdAsync(string userId)
        {
            // التأكد من أن userId ليس null أو فارغ
            if (string.IsNullOrEmpty(userId))
                return null;

            // الاستعلام عن العربة مع العناصر
            var cart = await _dbSet
                .Where(c => c.UserId == userId)
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync();

            // إذا لم يتم العثور على العربة
            if (cart == null)
                return null;

            // التأكد من أن cart.CartItems ليست null أو فارغة
            if (cart.CartItems == null || !cart.CartItems.Any())
            {
                cart.CartItems = new List<CartItem>(); // تأكد من أن CartItems ليست null
            }

            // تحميل تفاصيل المنتج لكل عنصر في السلة
            foreach (var item in cart.CartItems)
            {
                if (item != null)
                {
                    // استخدام FindAsync أو FirstOrDefaultAsync لاكتشاف المنتج المرتبط
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                    if (product != null)
                    {
                        item.Product = product; // ربط المنتج بالعنصر في السلة
                    }
                    else
                    {
                        // إذا لم يتم العثور على المنتج، يمكن تسجيل أو التعامل مع الحالة حسب الحاجة
                        item.Product = null; // يمكن أن تتركه null أو تعالج الحالة بطريقة أخرى
                    }
                }
            }

            return cart;
        }

    }
}
