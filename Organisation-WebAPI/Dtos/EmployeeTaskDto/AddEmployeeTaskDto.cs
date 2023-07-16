using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.EmployeeTaskDto
{
    public class AddEmployeeTaskDto
    {
        public string? TaskName { get; set; }
        public string? TaskDescription { get; set; }
        public DateTime? TaskCreatedDate { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public Status TaskStatus { get; set; } 
        public int EmployeeId { get; set; }
    }
}