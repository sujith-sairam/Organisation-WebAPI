using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult<ServiceResponse<int>>> GetProductRevenue()
        {
            var serviceResponse = await _dashboardService.GetTotalCount();
            return Ok(serviceResponse);
        }

        [HttpGet("GetChartDetails")]
        public async Task<ActionResult<ServiceResponse<int>>> GetChartDetails()
        {
            var serviceResponse = await _dashboardService.GetChartDetails();
            return Ok(serviceResponse);
        }
      
    }
}