using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string ?EmployeeName { get; set; }
        public int EmployeeSalary { get; set; }
        public int EmployeeAge { get; set; }
        [ForeignKey("User")]
        public int? UserID { get; set; }
        [ForeignKey("Manager")]
        public int ManagerID {get;set;}
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }

        [ForeignKey("Product")]
        public int ProductID {get; set;}

        [ForeignKey("Manager")]
        public int? ManagerID { get; set; }
        public Manager? Manager { get; set; }
        public User? User { get; set; }
        public Department? Department { get; set; }
        public Product? Product {get; set;}


    }
}