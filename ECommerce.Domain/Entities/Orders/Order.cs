using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Entities.Enums;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Domain.Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Payment Payment { get; set; }
        public Shipping Shipping { get; set; }
    }
}
