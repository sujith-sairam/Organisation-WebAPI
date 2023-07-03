using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.EmployeeTaskDto
{
    public class UpdateEmployeeTaskStatusDto
    {   
        public int EmployeeId { get; set; }
        public string? TaskStatus { get; set; } 
    }
}