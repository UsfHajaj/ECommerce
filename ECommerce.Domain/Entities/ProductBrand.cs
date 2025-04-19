using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class ProductBrand:BaseEntity
    {
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
