using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organisation_WebAPI.Dtos.EmployeeDto;
using Organisation_WebAPI.Dtos.ManagerDto;

namespace Organisation_WebAPI.Services.Employees
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<List<GetEmployeeDto>>> GetAllEmployees();
        Task<ServiceResponse<GetEmployeeDto>> GetEmployeeById(int id);
        Task<ServiceResponse<List<GetEmployeeDto>>> AddEmployee(AddEmployeeDto newDepartment);
        Task<ServiceResponse<UpdateEmployeeDto>> UpdateEmployee(UpdateEmployeeDto department,int id);
        Task<ServiceResponse<List<GetEmployeeDto>>> DeleteEmployee(int id);
        Task<ServiceResponse<List<GetEmployeeDto>>> GetAllEmployeesByManagerId(int managerId);
        //Task<ServiceResponse<List<GetEmployeeDto>>> GetAllEmployeesByProduct(int productId);
    }
}