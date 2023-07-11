using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.CustomerDto
{
    public class GetCustomerDto
    {
        public int CustomerID { get; set; }
        public string ?CustomerName { get; set; }
        public int CustomerPhoneNumber { get; set; }
        public string ?CustomerEmail {get;set;}
        public int ProductID {get; set;}
         public string? ProductName {get;set;}
        
    }
}