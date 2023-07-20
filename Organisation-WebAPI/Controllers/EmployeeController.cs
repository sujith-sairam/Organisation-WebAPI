using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
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
            var response = await _employeeService.GetAllEmployees();
            if(!response.Success)
            {
                BadRequest(response);
            }
            return Ok(response);
        }


        // Retrieves a employee from the database based on the provided ID
        [HttpGet("GetEmployeeById")]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> GetEmployee(int id)
        {
            var response = await _employeeService.GetEmployeeById(id);
            if (!response.Success)
            {
                BadRequest(response);
            }
            return Ok(response);
        }

        // Retrieves a employee from the database based on the provided ID
        [HttpGet("GetAllEmployeesByManagerId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> GetAllEmployeesByManagerId(int id)
        {
            var response = await _employeeService.GetAllEmployeesByManagerId(id);
            if (!response.Success)
            {
                BadRequest(response);
            }
            return Ok(response);
        }
        

        [HttpPut("UpdateEmployee")]
        public async Task<ActionResult<ServiceResponse<UpdateEmployeeDto>>> UpdateEmployee(UpdateEmployeeDto updatedEmployee,int id){
            var response = await _employeeService.UpdateEmployee(updatedEmployee,id);
            if (response.Success)
            {
                BadRequest(response);
            }
            return Ok(response);
        }
        
        // Deletes a employee from the database based on the provided ID
        [HttpDelete("DeleteEmployee")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> DeleteEmployee(int id){

            var response = await _employeeService.DeleteEmployee(id);
            if (response.Success)
            {
                BadRequest(response);
            }
            return Ok(response);

        }



    }
}