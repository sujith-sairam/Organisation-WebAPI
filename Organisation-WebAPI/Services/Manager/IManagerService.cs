using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organisation_WebAPI.Dtos.EmployeeDto;
using Organisation_WebAPI.Dtos.ManagerDto;

namespace Organisation_WebAPI.Services.Managers
{
    public interface IManagerService
    {
        Task<ServiceResponse<List<GetManagerDto>>> GetAllManagers();
        Task<ServiceResponse<GetManagerDto>> GetManagerById(int id);
        
        Task<ServiceResponse<List<GetManagerDto>>> AddManager(AddManagerDto newManager);
        Task<ServiceResponse<GetManagerDto>> UpdateManager(UpdateManagerDto updatedManager ,int id);
        Task<ServiceResponse<List<GetManagerDto>>> DeleteManager(int id);
        Task<ServiceResponse<GetManagerDto>> GetManagerByProductId(int productId); 
        Task<ServiceResponse<GetEmployeesAndManagerDto>> GetEmployeesAndManagerByProductId(int productId);
    }
}