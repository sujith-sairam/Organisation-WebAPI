using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.Dtos.EmployeeDto;
using Organisation_WebAPI.Dtos.ManagerDto;
using Organisation_WebAPI.Services.Employees;

namespace Organisation_WebAPI.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;  
        }

        // Retrieves all employees from the database
        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> GetEmployees()
        {
            return Ok(await _employeeService.GetAllEmployees());
        }


        // Retrieves a employee from the database based on the provided ID
        [HttpGet("GetEmployeeById")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> GetEmployee(int id)
        {
            return Ok(await _employeeService.GetEmployeeById(id));
        }

        // Retrieves a employee from the database based on the provided ID
        [HttpGet("GetAllEmployeesByManagerId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> GetAllEmployeesByManagerId(int id)
        {
            return Ok(await _employeeService.GetAllEmployeesByManagerId(id));
        }
        

        //[HttpGet("GetAllEmployeesByProduct")]
        //public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> GetAllEmployeesByProduct(int id)
        //{
        //    return Ok(await _employeeService.GetAllEmployeesByProduct(id));
        //}

        // Updates a employee in the database based on the provided ID
        [HttpPut("UpdateEmployee")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> UpdateEmployee(UpdateEmployeeDto updatedEmployee,int id){
            return Ok(await _employeeService.UpdateEmployee(updatedEmployee,id));
        }
        
        // Deletes a employee from the database based on the provided ID
        [HttpDelete("DeleteEmployee")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> DeleteEmployee(int id){
            return Ok(await _employeeService.DeleteEmployee(id));
        }



    }
}