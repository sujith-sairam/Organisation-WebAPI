using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.Dtos.DepartmentDto;
using Organisation_WebAPI.InputModels;
using Organisation_WebAPI.Services.Departments;
using Organisation_WebAPI.Services.Pagination;

namespace Organisation_WebAPI.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        

        // Retrieves all departments from the database

        [HttpGet("GetAllDepartment")]
        public async Task<ActionResult<ServiceResponse<GetDepartmentDto>>> GetDepartments()
        {

            var response = await _departmentService.GetAllDepartments();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        // Retrieves a department from the database based on the provided ID

        [HttpGet("GetDepartmentById")]
        public async Task<ActionResult<ServiceResponse<GetDepartmentDto>>> GetDepartment(int id)
        {
            var response = await _departmentService.GetDepartmentById(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

       

        // Adds a new Department to the database

        [HttpPost("CreateDepartment")]
        public async Task<ActionResult<ServiceResponse<GetDepartmentDto>>> AddProduct(AddDepartmentDto newDepartment)
        {

            var response = await _departmentService.AddDepartment(newDepartment);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        // Updates a department in the database based on the provided ID

        [HttpPut("UpdateDepartment")]
        public async Task<ActionResult<ServiceResponse<GetDepartmentDto>>> UpdateProduct(UpdateDepartmentDto updatedDepartment,int id){

            var response = await _departmentService.UpdateDepartment(updatedDepartment,id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        
        // Deletes a department from the database based on the provided ID

        [HttpDelete("DeleteDepartment")]
        public async Task<ActionResult<ServiceResponse<GetDepartmentDto>>> DeleteProduct(int id){

            var response = await _departmentService.DeleteDepartment(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        //Get Available Departments for Manager Appointment

        [HttpGet("GetAvailableDepartments")]
        public async Task<ActionResult<ServiceResponse<GetDepartmentDto>>> GetAvailableDepartments()
        {
            var response = await _departmentService.GetAvailableDepartments();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}