using Organisation_WebAPI.Dtos.DepartmentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.EmployeeDto
{
    public class GetEmployeeDto
    {
        public int EmployeeID { get; set; }
        public string ?EmployeeName { get; set; }
        public int EmployeeSalary { get; set; }
        public int EmployeeAge { get; set; }
        public string? ManagerName {get;set;}
        public string? DepartmentName { get; set; }
        public bool? ManagerIsAppointed { get; set;}


    }
}