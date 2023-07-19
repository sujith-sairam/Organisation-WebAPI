using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organisation_WebAPI.Data;
using Microsoft.EntityFrameworkCore;


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




       public async Task<ServiceResponse<Dictionary<string, int>>> GetTotalEmployeeCount()
       {
        var serviceResponse = new ServiceResponse<Dictionary<string, int>>();

        try
        {
        var departments = await _context.Departments.ToListAsync();

        var tableCounts = new Dictionary<string, int>();
        foreach (var department in departments)
        {
            var employeesCount = await _context.Employees.CountAsync(e => e.Manager!.DepartmentID == department.DepartmentID);
            tableCounts.Add(department.DepartmentName!, employeesCount);
        }

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