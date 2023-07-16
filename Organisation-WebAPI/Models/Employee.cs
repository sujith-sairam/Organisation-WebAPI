using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Models
{
    public class Employee
    {
        public Employee()
        {
            EmployeeTasks = new List<EmployeeTask>();
        }

        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
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
        public User? User { get; set; }
        public Department? Department { get; set; }
        public Product? Product {get; set;}
        public ICollection<EmployeeTask> EmployeeTasks { get; set; }


    }
}