using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organisation_WebAPI.Dtos.DashboardDto;

namespace Organisation_WebAPI.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<ServiceResponse<Dictionary<string,int>>> GetTotalCount();
    }
}