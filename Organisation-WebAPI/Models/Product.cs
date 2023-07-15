using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Models
{
    public class Product
    {
         public Product() { 
            Employees = new List<Employee>();
            Customers = new List<Customer>();
            Managers = new List<Manager>();
        }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int ProductRevenue {get;set;}
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Customer> Customers { get; set; }
        public ICollection<Manager> Managers {get;set;}
    }
}