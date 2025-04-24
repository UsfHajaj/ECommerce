using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        public List<OrderItemCreateDto> Items { get; set; }

        [Required]
        public PaymentCreateDto Payment { get; set; }

        [Required]
        public ShippingCreateDto Shipping { get; set; }
    }
}
