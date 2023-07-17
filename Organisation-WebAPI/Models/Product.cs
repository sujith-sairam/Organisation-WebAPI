using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Models
{
    public class Product
    {
        public int ProductID { get; set; }  
        public string? ProductName { get; set; }
        public int ProductRevenue {get;set;}
  
    }
}       
