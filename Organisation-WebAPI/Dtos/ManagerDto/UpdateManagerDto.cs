using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.ManagerDto
{
    public class UpdateManagerDto
    {
        public string? ManagerName {get;set;}
        public int ManagerSalary { get; set; }
        public int ManagerAge { get; set; }
    }
}