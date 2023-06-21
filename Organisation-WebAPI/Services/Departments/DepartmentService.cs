using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Dtos.DepartmentDto;

namespace Organisation_WebAPI.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly OrganizationContext _context ;
        

        public DepartmentService(IMapper mapper,OrganizationContext context)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public async Task<ServiceResponse<List<GetDepartmentDto>>> AddDepartment(AddDepartmentDto newDepartment)
        {
            var serviceResponse = new ServiceResponse<List<GetDepartmentDto>>();
            var department = _mapper.Map<Department>(newDepartment);

             _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Departments.Select(c => _mapper.Map<GetDepartmentDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetDepartmentDto>>> DeleteDepartment(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetDepartmentDto>>();
            try {

            var department = await _context.Departments.FirstOrDefaultAsync(c => c.DepartmentID == id);
            if (department is null)
                throw new Exception($"Character with id '{id}' not found");
            
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Departments.Select(c => _mapper.Map<GetDepartmentDto>(c)).ToList();
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
                return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetDepartmentDto>>> GetAllDepartments()
        {
            var serviceResponse = new ServiceResponse<List<GetDepartmentDto>>();
            var dbDepartments = await _context.Departments.ToListAsync();
            serviceResponse.Data = dbDepartments.Select(c => _mapper.Map<GetDepartmentDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetDepartmentDto>> GetDepartmentById(int id)
        {
            
            var serviceResponse = new ServiceResponse<GetDepartmentDto>();
            var dbDepartment =  await _context.Departments.FirstOrDefaultAsync(c => c.DepartmentID == id);
            serviceResponse.Data = _mapper.Map<GetDepartmentDto>(dbDepartment);
            return serviceResponse;
        }

        public Task<ServiceResponse<int>> GetDepartmentCount()
        {
            var serviceResponse = new ServiceResponse<int>();
            var count =  _context.Departments.Count();
            serviceResponse.Data = count;
            return Task.FromResult(serviceResponse);
        }

        public async Task<ServiceResponse<GetDepartmentDto>> UpdateDepartment(UpdateDepartmentDto updatedDepartment, int id)
        {
              var serviceResponse = new ServiceResponse<GetDepartmentDto>();
            try {
                var department = await _context.Departments.FirstOrDefaultAsync(c => c.DepartmentID == id);

                if (department is null)
                    throw new Exception($"Character with id '{id}' not found");
                
                department.DepartmentName = updatedDepartment.DepartmentName;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetDepartmentDto>(department);

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