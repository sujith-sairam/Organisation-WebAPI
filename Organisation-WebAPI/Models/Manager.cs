using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Organisation_WebAPI.Models
{
    public class Manager
    {
        [Key]
        public int ManagerId {get;set;}
        public string? ManagerName {get;set;}
        public int ManagerSalary { get; set; }
        public int ManagerAge { get; set; }
        [ForeignKey("Product")]
        public int ProductID {get;set;}
        public Product? Product {get;set;}
        

    }
}