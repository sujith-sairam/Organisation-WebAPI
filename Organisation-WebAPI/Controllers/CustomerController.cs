using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.Dtos.CustomerDto;
using Organisation_WebAPI.Services.Customers;

namespace Organisation_WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // Retrieves all customers from the database
        [HttpGet("GetAllCustomer")]
        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> GetCustomers()
        {
            return Ok(await _customerService.GetAllCustomers());
        }

        // Retrieves a customer from the database based on the provided ID
        [HttpGet("GetCustomerById")]
        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> GetDepartment(int id)
        {
            return Ok(await _customerService.GetCustomerById(id));
        }

        // Retrieves the count of customers in the database
        [HttpGet("GetCustomerCount")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCustomerCount()
        {
            var serviceResponse = await _customerService.GetCustomerCount();
            return Ok(serviceResponse);
        }

        // Adds a new customer to the database
        [HttpPost("CreateCustomer")]
        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> AddCustomer(AddCustomerDto newCustomer)
        {
            return Ok(await _customerService.AddCustomer(newCustomer));
        }

        // Updates a customer in the database based on the provided ID
        [HttpPut("UpdateCustomer")]
        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> UpdateCustomer(UpdateCustomerDto updatedCustomer,int id){
            return Ok(await _customerService.UpdateCustomer(updatedCustomer,id));
        }
        
        // Deletes a customer from the database based on the provided ID
        [HttpDelete("DeleteCustomer")]
        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> DeleteCustomer(int id){
            return Ok(await _customerService.DeleteCustomer(id));
        }
    }
}