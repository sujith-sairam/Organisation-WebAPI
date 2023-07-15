using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Organisation_WebAPI.Models
{
    public class Manager
    {
        public Manager()
        {
            Managers = new List<Manager>();
        }
        [Key]
        public int ManagerID {get;set;}
        public string? ManagerName {get;set; }
        public int ManagerSalary { get; set; }
        [ForeignKey("User")]
        public int? UserID { get; set; }
        public int ManagerAge { get; set; }
        [ForeignKey("Product")]
        public int ProductID {get;set;}
        public Product? Product {get;set;}
        public User? User { get; set; }

        public ICollection<Manager> Managers {get;set;}
    }
}