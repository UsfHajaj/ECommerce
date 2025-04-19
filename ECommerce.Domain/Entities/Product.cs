using ECommerce.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public int ProductBrandId { get; set; }
        // Navigation property
        public Category Category { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
