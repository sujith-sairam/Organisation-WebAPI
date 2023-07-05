using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.EmployeeTaskDto
{
    public class GetEmployeeTaskDto
    {
        public int Id { get; set; }
        public string? TaskName { get; set; }
        public string? TaskDescription { get; set; }
        public DateTime? TaskCreatedDate { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public string? TaskStatus { get; set; }  
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}