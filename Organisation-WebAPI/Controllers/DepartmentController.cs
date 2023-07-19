using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.Dtos.DepartmentDto;
using Organisation_WebAPI.Services.Departments;

namespace Organisation_WebAPI.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;

        }

        // Retrieves all departments from the database
        [HttpGet("GetAllDepartment")]
        //[Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetDepartmentDto>>> GetDepartments()
        {
            return Ok(await _departmentService.GetAllDepartments());
        }

        // Retrieves a department from the database based on the provided ID
        [HttpGet("GetDepartmentById")]
        public async Task<ActionResult<ServiceResponse<GetDepartmentDto>>> GetDepartment(int id)
        {
            return Ok(await _departmentService.GetDepartmentById(id));
        }

       

        // Adds a new Department to the database
        [HttpPost("CreateDepartment")]
        public async Task<ActionResult<ServiceResponse<GetDepartmentDto>>> AddProduct(AddDepartmentDto newDepartment)
        {
            return Ok(await _departmentService.AddDepartment(newDepartment));
        }

        // Updates a department in the database based on the provided ID
        [HttpPut("UpdateDepartment")]
        public async Task<ActionResult<ServiceResponse<GetDepartmentDto>>> UpdateProduct(UpdateDepartmentDto updatedDepartment,int id){
            return Ok(await _departmentService.UpdateDepartment(updatedDepartment,id));
        }
        
        // Deletes a department from the database based on the provided ID
        [HttpDelete("DeleteDepartment")]
        public async Task<ActionResult<ServiceResponse<GetDepartmentDto>>> DeleteProduct(int id){
            return Ok(await _departmentService.DeleteDepartment(id));
        }
    }
}