using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.Services.Dashboard;

namespace Organisation_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        
        [HttpGet("GetTotalCount")]
        //[Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<int>>> GetProductRevenue()
        {
            var response = await _dashboardService.GetTotalCount();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
      
    }
}