using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.CustomerDto
{
    public class UpdateCustomerDto
    {
        public string ?CustomerName { get; set; }
        public int CustomerPhoneNumber { get; set; }
        public string ?CustomerEmail {get;set;}
        public int ProductID {get; set;}
       
    }
}