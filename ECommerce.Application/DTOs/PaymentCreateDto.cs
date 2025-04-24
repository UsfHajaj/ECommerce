using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class PaymentCreateDto
    {
        [Required]
        public string PaymentMethod { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        // بيانات خاصة بالدفع (مثل معلومات البطاقة المشفرة، أو معرف النظام الخارجي)
        public string PaymentToken { get; set; }
    }
}
