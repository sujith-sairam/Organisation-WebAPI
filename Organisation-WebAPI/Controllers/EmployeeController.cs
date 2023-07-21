using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.Dtos.DepartmentDto;
using Organisation_WebAPI.Dtos.EmployeeDto;
using Organisation_WebAPI.Dtos.ManagerDto;
using Organisation_WebAPI.InputModels;
using Organisation_WebAPI.Services.Employees;
using Organisation_WebAPI.Services.Pagination;

namespace Organisation_WebAPI.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = nameof(UserRole.Admin))]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPaginationServices<GetEmployeeDto, Employee> _paginationServices;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeService employeeService, IPaginationServices<GetEmployeeDto, Employee> paginationServices, IMapper mapper)
        {
            _employeeService = employeeService;  
            _paginationServices = paginationServices;
            _mapper = mapper;
        }

        //[HttpPost]
        //[Route("GetPagination")]
        //public async Task<ActionResult> GetPagination([FromBody] PaginationInput paginationInput)
        //{
        //    var serviceResponse = await _employeeService.GetAllEmployees();
        //    var employeesDto = serviceResponse.Data;

        //    // Convert GetEmployeeDto to Employee using AutoMapper
        //    var employees = _mapper.Map<List<Employee>>(employeesDto);
        //    var result = _paginationServices.GetPagination(employees, paginationInput); 
        //    return Ok(result);
        //}

        // Retrieves all employees from the database
        [HttpPost("GetAllEmployees")]
        public async Task<ActionResult<ServiceResponse<GetEmployeeDto>>> GetEmployees(PaginationInput paginationInput)
        {
            var response = await _employeeService.GetAllEmployees(paginationInput);
            if(!response.Success)
            {
                BadRequest(response);
            }
            return Ok(response);
        }


        // Retrieves a employee from the database based on the provided ID
        [HttpGet("GetEmployeeById")]
        //[Authorize(Roles = nameof(UserRole.Manager))]
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
        [Authorize(Roles = nameof(UserRole.Admin) + "," + nameof(UserRole.Manager))]
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