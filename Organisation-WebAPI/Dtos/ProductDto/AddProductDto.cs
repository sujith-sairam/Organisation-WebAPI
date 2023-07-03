using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.ProductDto
{
    public class AddProductDto
    {
        public string? ProductName { get; set; }
        
        public int ProductRevenue {get;set;}
    }
}