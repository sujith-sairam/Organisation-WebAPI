using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string ?CustomerName { get; set; }
        public int CustomerPhoneNumber { get; set; }

        public string ?CustomerEmail {get;set;}

        [ForeignKey("Product")]
        public int ProductID {get; set;}
        
        public Product? Product { get; set; }
    }
}