﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class UpdateCartItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
