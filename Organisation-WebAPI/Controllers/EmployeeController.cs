using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.Dtos.EmployeeDto;
using Organisation_WebAPI.Services.Employees;

namespace Organisation_WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            
        }

          [HttpGet("GetAllEmployees")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> GetEmployees()
        {
            return Ok(await _employeeService.GetAllEmployees());
        }

        [HttpPost("CreateEmployee")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> AddEmployee(AddEmployeeDto newEmployee)
        {
            return Ok(await _employeeService.AddEmployee(newEmployee));
        }

        [HttpPut("UpdateEmployee")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> UpdateEmployee(UpdateEmployeeDto updatedEmployee,int id){
            return Ok(await _employeeService.UpdateEmployee(updatedEmployee,id));
        }
        
        [HttpDelete("DeleteEmployee")]

        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> DeleteEmployee(int id){
            return Ok(await _employeeService.DeleteEmployee(id));
        }
    }
}