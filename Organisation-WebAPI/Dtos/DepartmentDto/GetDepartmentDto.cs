using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Organisation_WebAPI.Dtos.DepartmentDto
{
    public class GetDepartmentDto
    {
        public int DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
    }
}