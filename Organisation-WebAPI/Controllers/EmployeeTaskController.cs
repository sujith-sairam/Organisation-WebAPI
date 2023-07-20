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
        //[Authorize(Roles = nameof(UserRole.Employee))]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetEmployeeTasks()
        {
            var response = await _employeeTaskService.GetAllEmployeeTasks(); 

            if(!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllEmployeeTasksByEmployeeId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetEmployeeTasksByEmployeeId(int id)
        {
            var response =  await _employeeTaskService.GetAllEmployeeTasksByEmployeeId(id);

            if(!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetNewEmployeeTasksByEmployeeId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetNewEmployeeTasks(int id)
        {
            var response = await _employeeTaskService.GetEmployeeNewTaskByEmployeeId(id);
            if(!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetOngoingEmployeeTasksByEmployeeId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetOngoingEmployeeTasks(int id)
        {
            var response =  await _employeeTaskService.GetEmployeeOngoingTaskByEmployeeId(id);

            if(!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetCompletedEmployeeTasksByEmployeeId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetCompletedEmployeeTasks(int id)
        {
            var response = await _employeeTaskService.GetEmployeeCompletedTaskByEmployeeId(id);

            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetPendingEmployeeTasksByManagerId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetPendingEmployeeTasksByEmployeeId(int id)
        {   
            var response = await _employeeTaskService.GetEmployeePendingTaskByEmployeeId(id);

            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetNewTaskCount")]
        public async Task<ActionResult<ServiceResponse<int>>> GetNewEmployeeTaskCount(int id){

            var response =  await _employeeTaskService.CalculateNewEmployeeTasksByEmployeeId(id);

            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("CreateEmployeeTasks")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> AddEmployeeTask(AddEmployeeTaskDto newEmployeeTask)
        {
            var response = await _employeeTaskService.AddEmployeeTask(newEmployeeTask);

            if(!response.Success) { 
                return BadRequest(response);
            }
            return Ok(response);
        }

        
        [HttpDelete("DeleteEmployeeTask")]
       // [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> DeleteEmployeeTask(int id){
            var response = await _employeeTaskService.DeleteEmployeeTask(id);

            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }
        
        [HttpGet("GetEmployeeTaskById")]
       // [Authorize(Roles = nameof(UserRole.Employee))]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> GetEmployeeTask(int id)
        {
            var response = await _employeeTaskService.GetEmployeeTaskById(id);

            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPut("UpdateEmployeeTask")]
       // [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> UpdateEmployeeTask(UpdateEmployeeTaskDto updatedEmployeeTask,int id){

            var response = await _employeeTaskService.UpdateEmployeeTask(updatedEmployeeTask,id);

            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPut("UpdateEmployeeTaskStatus")]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<GetEmployeeTaskDto>>> UpdateEmployeeTaskStatus(UpdateEmployeeTaskStatusDto updatedEmployeeTaskStatus,int id){
            var response = await _employeeTaskService.UpdateEmployeeTaskStatus(updatedEmployeeTaskStatus,id);

            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
            
        }
    }
}