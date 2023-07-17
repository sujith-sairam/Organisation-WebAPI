using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organisation_WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Organisation_WebAPI.Dtos.DashboardDto;

namespace Organisation_WebAPI.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {   
        private readonly OrganizationContext _context;
        private readonly ILogger<DashboardService> _logger;
        public DashboardService(OrganizationContext context , ILogger<DashboardService> logger)
        {
            _context = context;
            _logger = logger;
        }

   
        public async Task<ServiceResponse<Dictionary<string, int>>> GetTotalCount()
        {
           var serviceResponse = new ServiceResponse<Dictionary<string, int>>();

            try
            {
                var employeesList = await _context.Employees.ToListAsync();
                var managersList = await _context.Managers.ToListAsync();
                var departmentsList = await _context.Departments.ToListAsync();

                var tableCounts = new Dictionary<string, int>
                {
                    { "Employees", employeesList.Count },
                    { "Managers", managersList.Count },
                    { "Departments", departmentsList.Count }
                };

                serviceResponse.Data = tableCounts;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        
    }
}