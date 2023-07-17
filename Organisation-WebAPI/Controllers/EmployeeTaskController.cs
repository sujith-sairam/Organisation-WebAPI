using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.Dtos.EmployeeTaskDto;
using Organisation_WebAPI.Services.EmployeeTasks;

namespace Organisation_WebAPI.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeTaskController : ControllerBase
    {
        private readonly IEmployeeTaskService _employeeTaskService;

        public EmployeeTaskController(IEmployeeTaskService employeeTaskService)
        {
            _employeeTaskService = employeeTaskService;
        }

        [HttpGet("GetAllEmployeeTasks")]
        [Authorize(Roles = nameof(UserRole.Employee))]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetEmployeeTasks()
        {
            return Ok(await _employeeTaskService.GetAllEmployeeTasks());
        }

        [HttpGet("GetAllEmployeeTasksByEmployeeId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetEmployeeTasksByEmployeeId(int id)
        {
            return Ok(await _employeeTaskService.GetAllEmployeeTasksByEmployeeId(id));
        }

        [HttpGet("GetNewEmployeeTasksByEmployeeId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetNewEmployeeTasks(int id)
        {
            return Ok(await _employeeTaskService.GetEmployeeNewTaskByEmployeeId(id));
        }

        [HttpGet("GetOngoingEmployeeTasksByEmployeeId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetOngoingEmployeeTasks(int id)
        {
            return Ok(await _employeeTaskService.GetEmployeeOngoingTaskByEmployeeId(id));
        }

        [HttpGet("GetCompletedEmployeeTasksByEmployeeId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetCompletedEmployeeTasks(int id)
        {
            return Ok(await _employeeTaskService.GetEmployeeCompletedTaskByEmployeeId(id));
        }

        [HttpGet("GetPendingEmployeeTasksByManagerId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetPendingEmployeeTasksByEmployeeId(int id)
        {
            return Ok(await _employeeTaskService.GetEmployeePendingTaskByEmployeeId(id));
        }

        [HttpPost("CreateEmployeeTasks")]
        [Authorize(Roles = nameof(UserRole.Employee))]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> AddEmployeeTask(AddEmployeeTaskDto newEmployeeTask)
        {
            return Ok(await _employeeTaskService.AddEmployeeTask(newEmployeeTask));
        }

        
        [HttpDelete("DeleteEmployeeTask")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> DeleteEmployeeTask(int id){
            return Ok(await _employeeTaskService.DeleteEmployeeTask(id));
        }
        
        [HttpGet("GetEmployeeTaskById")]
        [Authorize(Roles = nameof(UserRole.Employee))]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetEmployeeTask(int id)
        {
            return Ok(await _employeeTaskService.GetEmployeeTaskById(id));
        }


        [HttpPut("UpdateEmployeeTask")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> UpdateEmployeeTask(UpdateEmployeeTaskDto updatedEmployeeTask,int id){
            return Ok(await _employeeTaskService.UpdateEmployeeTask(updatedEmployeeTask,id));
        }


        [HttpPut("UpdateEmployeeTaskStatus")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> UpdateEmployee(UpdateEmployeeTaskStatusDto updatedEmployeeTaskStatus,int id){
            return Ok(await _employeeTaskService.UpdateEmployeeTaskStatus(updatedEmployeeTaskStatus,id));
        }
    }
}