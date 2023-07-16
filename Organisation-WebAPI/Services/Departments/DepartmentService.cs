using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Dtos.DepartmentDto;
using Organisation_WebAPI.Dtos.EmployeeDto;

namespace Organisation_WebAPI.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IMapper _mapper;  // Provides object-object mapping
        private readonly OrganizationContext _context ; // Provides object-object mapping
        

        public DepartmentService(IMapper mapper,OrganizationContext context)
        {
            _context = context; // Injects the OrganizationContext instance
            _mapper = mapper; // Injects the IMapper instance
            
        }
         // Adds a new department to the database
        public async Task<ServiceResponse<List<GetDepartmentDto>>> AddDepartment(AddDepartmentDto newDepartment)
        {   
            var serviceResponse = new ServiceResponse<List<GetDepartmentDto>>();
            try 
            {
            var department = _mapper.Map<Department>(newDepartment);

             _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Departments.Select(c => _mapper.Map<GetDepartmentDto>(c)).ToListAsync();
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            } 
            return serviceResponse;
        }

        // Deletes a department from the database based on the provided ID
        public async Task<ServiceResponse<List<GetDepartmentDto>>> DeleteDepartment(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetDepartmentDto>>();
            try {

            var department = await _context.Departments.FirstOrDefaultAsync(c => c.DepartmentID == id);
            if (department is null)
                throw new Exception($"Department with id '{id}' not found");
            
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

        // Retrieves all department from the database
        public async Task<ServiceResponse<List<GetDepartmentDto>>> GetAllDepartments()
        {
            var serviceResponse = new ServiceResponse<List<GetDepartmentDto>>();
            try
            {
            var dbDepartments = await _context.Departments.ToListAsync();
            serviceResponse.Data = dbDepartments.Select(c => _mapper.Map<GetDepartmentDto>(c)).ToList();
            } 
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetEmployeeDto>>> GetEmployeesByDepartmentId(int departmentId)
        {
            var serviceResponse = new ServiceResponse<List<GetEmployeeDto>>();
            try
            {
                var department = await _context.Departments
                    .Include(d => d.Employees)
                    .FirstOrDefaultAsync(d => d.DepartmentID == departmentId);

                if (department == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Department not found.";
                    return serviceResponse;
                }

                var employees = department.Employees.ToList();
                serviceResponse.Data = employees.Select(e => _mapper.Map<GetEmployeeDto>(e)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }


        //Retrieves a department from the database with Id
        public async Task<ServiceResponse<GetDepartmentDto>> GetDepartmentById(int id)
        {
            
            var serviceResponse = new ServiceResponse<GetDepartmentDto>();
            try
            {
            var department =  await _context.Departments.FirstOrDefaultAsync(c => c.DepartmentID == id);
            if (department is null)
                    throw new Exception($"Department with id '{id}' not found");
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

        public async Task<ServiceResponse<GetDepartmentDto>> UpdateDepartment(UpdateDepartmentDto updatedDepartment, int id)
        {
            var serviceResponse = new ServiceResponse<GetDepartmentDto>();
            try {
                var department = await _context.Departments.FirstOrDefaultAsync(c => c.DepartmentID == id);

                if (department is null)
                    throw new Exception($"Department with id '{id}' not found");
                
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