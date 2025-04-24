using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class ShippingDto
    {
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string Status { get; set; }
    }
}
