using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organisation_WebAPI.Dtos.EmployeeTaskDto;

namespace Organisation_WebAPI.Services.EmployeeTasks
{
    public interface IEmployeeTaskService
    {
        Task<ServiceResponse<List<GetEmployeeTaskDto>>> GetAllEmployeeTasks();
        Task<ServiceResponse<GetEmployeeTaskDto>> GetEmployeeTaskById(int id);
        Task<ServiceResponse<List<GetEmployeeTaskDto>>> AddEmployeeTask(AddEmployeeTaskDto addEmployeeTask);
        Task<ServiceResponse<GetEmployeeTaskDto>> UpdateEmployeeTask(UpdateEmployeeTaskDto updateEmployeeTask,int id);
        Task<ServiceResponse<GetEmployeeTaskDto>> UpdateEmployeeTaskStatus(UpdateEmployeeTaskStatusDto updateEmployeeTaskStatus,int id);
        Task<ServiceResponse<List<GetEmployeeTaskDto>>> DeleteEmployeeTask(int id);

    }
}