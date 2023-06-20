using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Models
{
    public class Department
    {
         public Department() { 
            Employees = new List<Employee>();
        }
        public int DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}