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

        public DashboardService(OrganizationContext context )
        {
            _context = context;
        
        }

     public async Task<ServiceResponse<Dictionary<Status, int>>> GetEmployeeTaskCount(int id)
     {
        var serviceResponse = new ServiceResponse<Dictionary<Status, int>>();

        try
        {
            var allStatuses = Enum.GetValues(typeof(Status)).Cast<Status>();
            var taskStatusCounts = await _context.EmployeeTasks
            .Where(employee => employee.EmployeeId == id)
            .GroupBy(employee => employee.TaskStatus)
            .ToDictionaryAsync(group => group.Key, group => group.Count());

            var finalStatusCounts = allStatuses.ToDictionary(status => status, status => taskStatusCounts.GetValueOrDefault(status, 0));

            serviceResponse.Data = finalStatusCounts;
            serviceResponse.Message = "Task status counts retrieved successfully.";
        }
        catch (Exception ex)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = "Failed to retrieve task status counts: " + ex.Message;
        }

            return serviceResponse;
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