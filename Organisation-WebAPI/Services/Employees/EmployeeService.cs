using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Dtos.EmployeeDto;

namespace Organisation_WebAPI.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly OrganizationContext _context ;
        

        public EmployeeService(IMapper mapper,OrganizationContext context)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public async Task<ServiceResponse<List<GetEmployeeDto>>> AddEmployee(AddEmployeeDto newEmployee)
        {
            var serviceResponse = new ServiceResponse<List<GetEmployeeDto>>();
            var employee = _mapper.Map<Employee>(newEmployee);

             _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Employees.Select(c => _mapper.Map<GetEmployeeDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetEmployeeDto>>> DeleteEmployee(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetEmployeeDto>>();
            try {

            var employee = await _context.Employees.FirstOrDefaultAsync(c => c.EmployeeID == id);
            if (employee is null)
                throw new Exception($"Character with id '{id}' not found");
            
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Employees.Select(c => _mapper.Map<GetEmployeeDto>(c)).ToList();
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
                return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetEmployeeDto>>> GetAllEmployees()
        {
            var serviceResponse = new ServiceResponse<List<GetEmployeeDto>>();
            var dbEmployees = await _context.Employees.ToListAsync();
            serviceResponse.Data = dbEmployees.Select(c => _mapper.Map<GetEmployeeDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetEmployeeDto>> UpdateEmployee(UpdateEmployeeDto updatedEmployee, int id)
        {
            var serviceResponse = new ServiceResponse<GetEmployeeDto>();
            try {
                var employee = await _context.Employees.FirstOrDefaultAsync(c => c.EmployeeID == id);

                if (employee is null)
                    throw new Exception($"Character with id '{id}' not found");
                
                employee.EmployeeName = updatedEmployee.EmployeeName;
                employee.EmployeeSalary = updatedEmployee.EmployeeSalary;
                employee.DepartmentID = updatedEmployee.DepartmentID;
                employee.EmployeeAge = updatedEmployee.EmployeeAge;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetEmployeeDto>(employee);

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