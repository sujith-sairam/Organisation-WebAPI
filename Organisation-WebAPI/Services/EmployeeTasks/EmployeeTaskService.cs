using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Dtos.EmployeeTaskDto;


namespace Organisation_WebAPI.Services.EmployeeTasks
{
    public class EmployeeTaskService : IEmployeeTaskService
    {
        private readonly IMapper _mapper;  // Provides object-object mapping
        private readonly OrganizationContext _context ; // Represents the database context

        public EmployeeTaskService(IMapper mapper,OrganizationContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResponse<List<GetEmployeeTaskDto>>> AddEmployeeTask(AddEmployeeTaskDto addEmployeeTask)
        {
            var serviceResponse = new ServiceResponse<List<GetEmployeeTaskDto>>();
            var employeeTask = _mapper.Map<EmployeeTask>(addEmployeeTask);

             _context.EmployeeTasks.Add(employeeTask);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.EmployeeTasks.Select(c => _mapper.Map<GetEmployeeTaskDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetEmployeeTaskDto>>> DeleteEmployeeTask(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetEmployeeTaskDto>>();
            try {

            var employeeTask = await _context.EmployeeTasks.FirstOrDefaultAsync(c => c.TaskID == id);
            if (employeeTask is null)
                throw new Exception($"EmployeeTask with id '{id}' not found");
            
            _context.EmployeeTasks.Remove(employeeTask);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.EmployeeTasks.Select(c => _mapper.Map<GetEmployeeTaskDto>(c)).ToList();
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
                return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetEmployeeTaskDto>>> GetAllEmployeeTasks()
        {
            var serviceResponse = new ServiceResponse<List<GetEmployeeTaskDto>>();
            var dbEmployeeTasks = await _context.EmployeeTasks.ToListAsync();
            serviceResponse.Data = dbEmployeeTasks.Select(c => _mapper.Map<GetEmployeeTaskDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetEmployeeTaskDto>> GetEmployeeTaskById(int id)
        {
             
            var serviceResponse = new ServiceResponse<GetEmployeeTaskDto>();
            try
            {
            var dbEmployeeTask =  await _context.EmployeeTasks.FirstOrDefaultAsync(c => c.TaskID == id);
            if (dbEmployeeTask is null)
                    throw new Exception($"EmployeeTask with id '{id}' not found");

            serviceResponse.Data = _mapper.Map<GetEmployeeTaskDto>(dbEmployeeTask);
            return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public Task<ServiceResponse<int>> GetEmployeeTaskCount()
        {
            var serviceResponse = new ServiceResponse<int>();
            var count =  _context.EmployeeTasks.Count();
            serviceResponse.Data = count;
            return Task.FromResult(serviceResponse);
        }

        public async Task<ServiceResponse<GetEmployeeTaskDto>> UpdateEmployeeTask(UpdateEmployeeTaskDto updateEmployeeTask, int id)
        {
            var serviceResponse = new ServiceResponse<GetEmployeeTaskDto>();
            try {
                var employeeTask = await _context.EmployeeTasks.FirstOrDefaultAsync(c => c.TaskID == id);

                if (employeeTask is null)
                    throw new Exception($"EmployeeTask with id '{id}' not found");
                
                employeeTask.TaskName = updateEmployeeTask.TaskName;
                employeeTask.TaskDueDate = updateEmployeeTask.TaskDueDate;
                employeeTask.TaskDescription = updateEmployeeTask.TaskDescription;
                employeeTask.TaskStatus = updateEmployeeTask.TaskStatus;
                employeeTask.EmployeeId = updateEmployeeTask.EmployeeId;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetEmployeeTaskDto>(employeeTask);

                return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            
            return serviceResponse;   
        }    
        public async Task<ServiceResponse<GetEmployeeTaskDto>> UpdateEmployeeTaskStatus(UpdateEmployeeTaskStatusDto updateEmployeeTaskStatus, int id)
        {
            var serviceResponse = new ServiceResponse<GetEmployeeTaskDto>();
            try {
                var employeeTask = await _context.EmployeeTasks.FirstOrDefaultAsync(c => c.TaskID == id);

                if (employeeTask is null)
                    throw new Exception($"Employee with id '{id}' not found");
                
                
                employeeTask.TaskStatus = updateEmployeeTaskStatus.TaskStatus;
                

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetEmployeeTaskDto>(employeeTask);

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