using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Cart
{
    public class Cart:BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<CartItem> CartItems { get; set; }
    }
}
