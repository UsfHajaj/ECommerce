using ECommerce.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Cart
{
    public class CartItem:BaseEntity
    {
        public int cartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
    }
}
