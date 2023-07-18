using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organisation_WebAPI.Dtos.DepartmentDto;
using Organisation_WebAPI.Dtos.EmployeeDto;

namespace Organisation_WebAPI.Services.Departments
{
    public interface IDepartmentService
    {
        Task<ServiceResponse<List<GetDepartmentDto>>> GetAllDepartments();
        Task<ServiceResponse<GetDepartmentDto>> GetDepartmentById(int id);
        Task<ServiceResponse<List<GetDepartmentDto>>> AddDepartment(AddDepartmentDto newDepartment);
        Task<ServiceResponse<GetDepartmentDto>> UpdateDepartment(UpdateDepartmentDto department,int id);
        Task<ServiceResponse<List<GetDepartmentDto>>> DeleteDepartment(int id);
    }
}