using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Dtos.CustomerDto;

namespace Organisation_WebAPI.Services.Customers
{
    public class CustomerService : ICustomerService
    {   
        private readonly IMapper _mapper;  // Provides object-object mapping
        private readonly OrganizationContext _context;  // Provides object-object mapping

        public CustomerService(IMapper mapper,OrganizationContext context)
        {
            _mapper = mapper; // Injects the IMapper instance
            _context = context; // Injects the OrganizationContext instance
        }

        // Adds a new customer to the database
        public async Task<ServiceResponse<List<GetCustomerDto>>> AddCustomer(AddCustomerDto addCustomer)
        {
            var serviceResponse = new ServiceResponse<List<GetCustomerDto>>();
            var customer = _mapper.Map<Customer>(addCustomer);

             _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Customers.Select(c => _mapper.Map<GetCustomerDto>(c)).ToListAsync();
            return serviceResponse;
        }

        // Deletes a customer from the database based on the provided ID
        public async Task<ServiceResponse<List<GetCustomerDto>>> DeleteCustomer(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCustomerDto>>();
            try {

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerID == id);
            if (customer is null)
                throw new Exception($"Customer with id '{id}' not found");
            
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Customers.Select(c => _mapper.Map<GetCustomerDto>(c)).ToListAsync();
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
                return serviceResponse;
        }

        // Retrieves all customers from the database
        public async Task<ServiceResponse<List<GetCustomerDto>>> GetAllCustomers()
        {  
            var serviceResponse = new ServiceResponse<List<GetCustomerDto>>();
            var dbCustomers = await _context.Customers.ToListAsync();
            serviceResponse.Data = dbCustomers.Select(c => _mapper.Map<GetCustomerDto>(c)).ToList();
            return serviceResponse;
        }

        //Retrieves a customer from the database with Id
        public async Task<ServiceResponse<GetCustomerDto>> GetCustomerById(int id)
        {
            
            var serviceResponse = new ServiceResponse<GetCustomerDto>();
            try
            {
            var customer =  await _context.Customers.FirstOrDefaultAsync(c => c.CustomerID == id);
            if (customer is null)
                    throw new Exception($"Customer with id '{id}' not found");
            serviceResponse.Data = _mapper.Map<GetCustomerDto>(customer);
            return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
        return serviceResponse;
        }

        public Task<ServiceResponse<int>> GetCustomerCount()
        {
            var serviceResponse = new ServiceResponse<int>();
            var count =  _context.Customers.Count();
            serviceResponse.Data = count;
            return Task.FromResult(serviceResponse);
        }

        public async Task<ServiceResponse<GetCustomerDto>> UpdateCustomer(UpdateCustomerDto updatedCustomer, int id)
        {
             var serviceResponse = new ServiceResponse<GetCustomerDto>();
            try {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerID == id);
                if (customer is null)
                    throw new Exception($"Customer with id '{id}' not found");
                
                
                customer.CustomerName = updatedCustomer.CustomerName;
                customer.CustomerEmail = updatedCustomer.CustomerEmail;
                customer.CustomerPhoneNumber = updatedCustomer.CustomerPhoneNumber;
               

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCustomerDto>(customer);

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