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

        [HttpGet("GetAllCustomer")]
        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> GetCustomers()
        {
            return Ok(await _customerService.GetAllCustomers());
        }

        [HttpGet("GetCustomerById")]
        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> GetDepartment(int id)
        {
            return Ok(await _customerService.GetCustomerById(id));
        }

        [HttpGet("GetCustomerCount")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCustomerCount()
        {
            var serviceResponse = await _customerService.GetCustomerCount();
            return Ok(serviceResponse);
        }

        [HttpPost("CreateCustomer")]
        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> AddCustomer(AddCustomerDto newCustomer)
        {
            return Ok(await _customerService.AddCustomer(newCustomer));
        }

        [HttpPut("UpdateCustomer")]
        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> UpdateCustomer(UpdateCustomerDto updatedCustomer,int id){
            return Ok(await _customerService.UpdateCustomer(updatedCustomer,id));
        }
        
        [HttpDelete("DeleteCustomer")]

        public async Task<ActionResult<ServiceResponse<GetCustomerDto>>> DeleteCustomer(int id){
            return Ok(await _customerService.DeleteCustomer(id));
        }
    }
}