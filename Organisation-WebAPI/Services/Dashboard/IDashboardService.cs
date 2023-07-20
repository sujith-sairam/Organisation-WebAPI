using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Organisation_WebAPI.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<ServiceResponse<Dictionary<string,int>>> GetTotalEmployeeCount();
        Task<ServiceResponse<Dictionary<Status,int>>> GetEmployeeTaskCount(int id);
    }
}