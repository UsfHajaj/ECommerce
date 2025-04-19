using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Payments
{
    public class Shipping:BaseEntity
    {
        public string TrackingNumber { get; set; }
        public DateTime ShippingDate { get; set; }
        public string Status { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
