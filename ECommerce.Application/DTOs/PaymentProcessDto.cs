using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class PaymentProcessDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        public string PaymentToken { get; set; }
    }
}
