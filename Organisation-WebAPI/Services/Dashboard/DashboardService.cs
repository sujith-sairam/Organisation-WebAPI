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

        public async Task<ServiceResponse<List<OverViewChartDto>>> GetChartDetails()
        {
            var serviceResponse = new ServiceResponse<List<OverViewChartDto>>();

            var products = await _context.Products
            .Select(p => new OverViewChartDto
            {
                ProductName = p.ProductName,
                EmployeeCount = p.Employees.Count(),
                CustomerCount = p.Customers.Count(),
                ProductRevenue = p.ProductRevenue
            })
            .ToListAsync();

            serviceResponse.Data = products;
            return serviceResponse;
        }

        public async Task<ServiceResponse<Dictionary<string, int>>> GetTotalCount()
        {
           var serviceResponse = new ServiceResponse<Dictionary<string, int>>();

            try
            {
                var productsList = await _context.Products.ToListAsync();
                var employeesList = await _context.Employees.ToListAsync();
                var customersList = await _context.Customers.ToListAsync();
                var managersList = await _context.Managers.ToListAsync();
                var departmentsList = await _context.Departments.ToListAsync();

                var tableCounts = new Dictionary<string, int>
                {
                    { "Products", productsList.Count },
                    { "Employees", employeesList.Count },
                    { "Customers", customersList.Count },
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