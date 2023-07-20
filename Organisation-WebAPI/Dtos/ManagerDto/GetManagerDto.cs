using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.ManagerDto
{
    public class GetManagerDto
    {
        public int ManagerId {get;set;}
        public string? ManagerName {get;set;}
        public int ManagerSalary { get; set; }
        public int ManagerAge { get; set; }
        public int DepartmentID { get; set; }
        public bool IsAppointed { get; set; }
        public string? DepartmentName {get;set;}
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}