using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.EmployeeDto
{
    public class UpdateEmployeeDto
    {
        public string ?EmployeeName { get; set; }
        public int EmployeeSalary { get; set; }
        public int EmployeeAge { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string? Designation { get; set; }
        public int ManagerID {get;set;}
    }
}