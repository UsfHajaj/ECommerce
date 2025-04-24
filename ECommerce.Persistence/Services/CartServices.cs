using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities.Cart;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Services
{
    public class CartServices : ICartServices
    {
        private readonly ApplicationDbContext _context;
        public CartServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CartDto> GetCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                return null!;
            var cartDto = new CartDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = cart.CartItems.Sum(i => i.Quantity * i.Price),
                CartItems = cart.CartItems.Select(item => new CartItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    TotalPrice = item.Price * item.Quantity,
                    ProductImageUrl = item.Product.ImageUrl
                }).ToList()
            };

            return cartDto;
        }
        public async Task<CartItemDto> AddItemToCartAsync(string userId, AddCartItemDto addCartItemDto)
        {
            var product = await _context.Products.FindAsync(addCartItemDto.ProductId);
            if(product == null)
                throw new Exception($"this is not vailed {addCartItemDto.ProductId}");

            var cart = await _context.Carts
                .Include(m=>m.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if(cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                _context.Carts.Add(cart);
            }
            var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == addCartItemDto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += addCartItemDto.Quantity;
            }
            else
            {
                // إضافة عنصر جديد إلى السلة
                existingItem = new CartItem
                {
                    ProductId = product.Id,
                    Price = product.Price,
                    Quantity = addCartItemDto.Quantity,
                };
                cart.CartItems.Add(existingItem);
            }
            await _context.SaveChangesAsync();

            return new CartItemDto
            {
                Id = existingItem.Id,
                ProductId = existingItem.ProductId,
                ProductName = product.Name,
                Price = existingItem.Price,
                Quantity = existingItem.Quantity,
                TotalPrice = existingItem.Price * existingItem.Quantity,
                ProductImageUrl = product.ImageUrl
            };
        }
        public async Task<CartItemDto> UpdateCartItemAsync(string userId, UpdateCartItemDto updateCartItemDto)
        {
            var cart =await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                return null!;
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == updateCartItemDto.Id);
            if (cartItem == null)
                return null!;
            cartItem.Quantity = updateCartItemDto.Quantity;
            await _context.SaveChangesAsync();

            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.Name,
                Price = cartItem.Price,
                Quantity = cartItem.Quantity,
                TotalPrice = cartItem.Price * cartItem.Quantity,
                ProductImageUrl = cartItem.Product.ImageUrl
            };
        }

        public async Task<bool> RemoveCartItemAsync(string userId, int cartItemId)
        {
            var cart= await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                return false;
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null)
                return false;
            cart.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        
    }
}
