using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class ReviewDto
    {
        public int ProductId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
