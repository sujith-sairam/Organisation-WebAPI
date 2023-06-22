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
        private readonly IMapper _mapper;  // Provides object-object mapping
        private readonly OrganizationContext _context ; // Represents the database context
        

        public EmployeeService(IMapper mapper,OrganizationContext context)
        {
            _context = context; // Injects the OrganizationContext instance
            _mapper = mapper; // Injects the IMapper instance
            
        }

         // Adds a new Employee to the database
        public async Task<ServiceResponse<List<GetEmployeeDto>>> AddEmployee(AddEmployeeDto newEmployee)
        {
            var serviceResponse = new ServiceResponse<List<GetEmployeeDto>>();
            var employee = _mapper.Map<Employee>(newEmployee);

             _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Employees.Select(c => _mapper.Map<GetEmployeeDto>(c)).ToListAsync();
            return serviceResponse;
        }

        // Deletes a employee from the database based on the provided ID
        public async Task<ServiceResponse<List<GetEmployeeDto>>> DeleteEmployee(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetEmployeeDto>>();
            try {

            var employee = await _context.Employees.FirstOrDefaultAsync(c => c.EmployeeID == id);
            if (employee is null)
                throw new Exception($"Employee with id '{id}' not found");
            
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

        // Retrieves all employees from the database
        public async Task<ServiceResponse<List<GetEmployeeDto>>> GetAllEmployees()
        {
            var serviceResponse = new ServiceResponse<List<GetEmployeeDto>>();
            var dbEmployees = await _context.Employees.ToListAsync();
            serviceResponse.Data = dbEmployees.Select(c => _mapper.Map<GetEmployeeDto>(c)).ToList();
            return serviceResponse;
        }

        //Retrieves a employee from the database with Id
         public async Task<ServiceResponse<GetEmployeeDto>> GetEmployeeById(int id)
        {
            
            var serviceResponse = new ServiceResponse<GetEmployeeDto>();
            try
            {
            var dbEmployee =  await _context.Employees.FirstOrDefaultAsync(c => c.EmployeeID == id);
            if (dbEmployee is null)
                    throw new Exception($"Employee with id '{id}' not found");

            serviceResponse.Data = _mapper.Map<GetEmployeeDto>(dbEmployee);
            return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public Task<ServiceResponse<int>> GetEmployeeCount()
        {
            var serviceResponse = new ServiceResponse<int>();
            var count =  _context.Employees.Count();
            serviceResponse.Data = count;
            return Task.FromResult(serviceResponse);
        }

        public async Task<ServiceResponse<GetEmployeeDto>> UpdateEmployee(UpdateEmployeeDto updatedEmployee, int id)
        {
            var serviceResponse = new ServiceResponse<GetEmployeeDto>();
            try {
                var employee = await _context.Employees.FirstOrDefaultAsync(c => c.EmployeeID == id);

                if (employee is null)
                    throw new Exception($"Employee with id '{id}' not found");
                
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