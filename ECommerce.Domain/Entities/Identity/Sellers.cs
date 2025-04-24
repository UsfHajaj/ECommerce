using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Identity
{
    public class Sellers:ApplicationUser
    {
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public ProductBrand ProductBrand { get; set; }
    }
}
