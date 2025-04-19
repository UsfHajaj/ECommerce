using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Review: BaseEntity
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        // Navigation properties
        public Product Product { get; set; }
        public ApplicationUser User { get; set; }
    }
}
