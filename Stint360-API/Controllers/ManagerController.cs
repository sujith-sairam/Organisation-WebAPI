using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.Dtos.EmployeeDto;
using Organisation_WebAPI.Dtos.ManagerDto;
using Organisation_WebAPI.InputModels;
using Organisation_WebAPI.Services.Managers;

namespace Organisation_WebAPI.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
             _managerService = managerService;
        }

        [HttpPost("GetAllManagers")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetManagerDto>>> GetManagers(PaginationInput paginationInput)
        {
            var response = await _managerService.GetAllManagers(paginationInput);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetEmployeesAndManagerByDepartmentId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeesAndManagerDto>>> GetEmployeesAndManagerByDepartmentId(int id)
        {
            var response = await _managerService.GetEmployeesAndManagerByDepartmentId(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }



        [HttpGet("GetManagerById")]
        public async Task<ActionResult<ServiceResponse<GetManagerDto>>> GetManagerById(int id)
        {
            var response = await _managerService.GetManagerById(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetManagerByDepartmentId")]
        public async Task<ActionResult<ServiceResponse<GetManagerDto>>> GetManagerByDepartmentId(int id)
        {
            var response = await _managerService.GetManagerByDepartmentId(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateManager")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetManagerDto>>> UpdateManager(UpdateManagerDto updatedManager,int id){
            var response = await _managerService.UpdateManager(updatedManager,id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteManager")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetManagerDto>>> DeleteManager(int id){
            var response = await _managerService.DeleteManager(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllDepartmentsAssociatedWithManager")]
        public async Task<ActionResult<ServiceResponse<string>>> GetAllDepartmentsAssociatedWithManager()
        {
            var response = await _managerService.GetAllDepartmentsAssociatedWithManager();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}