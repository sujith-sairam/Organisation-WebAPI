using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Dtos.EmployeeDto;
using Organisation_WebAPI.Dtos.ManagerDto;


namespace Organisation_WebAPI.Services.Managers
{
    public class ManagerService : IManagerService
    {   

        private readonly IMapper _mapper;  // Provides object-object mapping
        private readonly OrganizationContext _context ; // Represents the database context

        public ManagerService(OrganizationContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetManagerDto>>> AddManager(AddManagerDto newManager)
        {
            var serviceResponse = new ServiceResponse<List<GetManagerDto>>();
            try {
            var manager = _mapper.Map<Manager>(newManager);
             _context.Managers.Add(manager);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Managers.Select(c => _mapper.Map<GetManagerDto>(c)).ToListAsync();
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;   
        }    


        public async Task<ServiceResponse<List<GetManagerDto>>> DeleteManager(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetManagerDto>>();
            try {

            var manager = await _context.Managers.FirstOrDefaultAsync(c => c.ManagerId == id);
            if (manager is null)
                throw new Exception($"Manager with id '{id}' not found");
            
            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Managers.Select(c => _mapper.Map<GetManagerDto>(c)).ToList();
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
                return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetManagerDto>>> GetAllManagers()
        {   
            var serviceResponse = new ServiceResponse<List<GetManagerDto>>();
            try
            {
            var dbManagers = await _context.Managers.ToListAsync();
            var managerDTOs = dbManagers.Select(e => new GetManagerDto
            {
                ManagerId = e.ManagerId,
                ManagerName = e.ManagerName,
                ManagerSalary = e.ManagerSalary,
                ManagerAge = e.ManagerAge,
                DepartmentID = e.DepartmentID,
                isAppointed = e.isAppointed,
                DepartmentName = _context.Departments.FirstOrDefault(d => d.DepartmentID == e.DepartmentID)?.DepartmentName
            }).ToList();

            serviceResponse.Data = managerDTOs;
            }

            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetManagerDto>> GetManagerByDepartmentId(int departmentId)
        {
            var serviceResponse = new ServiceResponse<GetManagerDto>();
            try
            {
                var manager = await _context.Managers
                    .Include(m => m.Department)
                    .FirstOrDefaultAsync(m => m.DepartmentID == departmentId);

                if (manager == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Manager not found.";
                    return serviceResponse;
                }

                var managerDto = _mapper.Map<GetManagerDto>(manager);
                managerDto.DepartmentName = manager.Department?.DepartmentName;

                serviceResponse.Data = managerDto;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetEmployeesAndManagerDto>> GetEmployeesAndManagerByDepartmentId(int departmentId)
        {
            var serviceResponse = new ServiceResponse<GetEmployeesAndManagerDto>();
            try
            {
                var manager = await _context.Managers
                    .Include(m => m.Department)
                    .Include(m => m.Employees)
                        .ThenInclude(e => e.Department)
                    .FirstOrDefaultAsync(m => m.DepartmentID == departmentId);

                if (manager == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Manager not found.";
                    return serviceResponse;
                }

                var managerDto = _mapper.Map<GetEmployeesAndManagerDto>(manager);


                serviceResponse.Data = managerDto;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetManagerDto>> GetManagerById(int id)
        {
            var serviceResponse = new ServiceResponse<GetManagerDto>();
            try
            {
            var dbManager =  await _context.Managers.FirstOrDefaultAsync(c => c.ManagerId == id);
            if (dbManager is null)
                    throw new Exception($"Manager with id '{id}' not found");

            serviceResponse.Data = _mapper.Map<GetManagerDto>(dbManager);
            return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }



        public async Task<ServiceResponse<GetManagerDto>> UpdateManager(UpdateManagerDto updatedManager, int id)
        {
            var serviceResponse = new ServiceResponse<GetManagerDto>();
            try {
                var manager = await _context.Managers.FirstOrDefaultAsync(c => c.ManagerId == id);

                if (manager is null)
                    throw new Exception($"Manager with id '{id}' not found");
                
                var departmentExists = await _context.Departments.AnyAsync(d => d.DepartmentID == updatedManager.DepartmentID);

                if (!departmentExists)
                    throw new Exception($"Invalid DepartmentID '{updatedManager.DepartmentID}'");

                manager.ManagerName = updatedManager.ManagerName;
                manager.ManagerSalary = updatedManager.ManagerSalary;
                manager.ManagerAge = updatedManager.ManagerAge;
                manager.DepartmentID = updatedManager.DepartmentID;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetManagerDto>(manager);

                return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            
            return serviceResponse;
        }
    }
}