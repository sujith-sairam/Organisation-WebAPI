using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.Dtos.EmployeeDto;
using Organisation_WebAPI.Dtos.ManagerDto;
using Organisation_WebAPI.Services.Managers;

namespace Organisation_WebAPI.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
             _managerService = managerService;
        }

        [HttpGet("GetAllManagers")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetManagerDto>>> GetManagers()
        {
            return Ok(await _managerService.GetAllManagers());
        }

        [HttpGet("GetEmployeesAndManagerByProductId")]
        public async Task<ActionResult<ServiceResponse<GetEmployeesAndManagerDto>>> GetEmployeesAndManagerByProductId(int id)
        {
            return Ok(await _managerService.GetEmployeesAndManagerByProductId(id));
        }



        [HttpGet("GetManagerById")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetManagerDto>>> GetEmployee(int id)
        {
            return Ok(await _managerService.GetManagerById(id));
        }

        [HttpGet("GetManagerByProductId")]
        public async Task<ActionResult<ServiceResponse<GetManagerDto>>> GetManagerByProductId(int id)
        {
            return Ok(await _managerService.GetManagerByProductId(id));
        }




        [HttpPut("UpdateManager")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetManagerDto>>> UpdateEmployee(UpdateManagerDto updatedManager,int id){
            return Ok(await _managerService.UpdateManager(updatedManager,id));
        }
        
        
        [HttpDelete("DeleteManager")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<GetManagerDto>>> DeleteEmployee(int id){
            return Ok(await _managerService.DeleteManager(id));
        }
    }
}