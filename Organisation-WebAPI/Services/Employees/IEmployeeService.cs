using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organisation_WebAPI.Dtos.EmployeeDto;

namespace Organisation_WebAPI.Services.Employees
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<List<GetEmployeeDto>>> GetAllEmployees();
        Task<ServiceResponse<List<GetEmployeeDto>>> AddEmployee(AddEmployeeDto newDepartment);
        Task<ServiceResponse<GetEmployeeDto>> UpdateEmployee(UpdateEmployeeDto department,int id);
        Task<ServiceResponse<List<GetEmployeeDto>>> DeleteEmployee(int id);
    }
}