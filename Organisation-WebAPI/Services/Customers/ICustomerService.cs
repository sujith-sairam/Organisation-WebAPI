using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organisation_WebAPI.Dtos.CustomerDto;

namespace Organisation_WebAPI.Services.Customers
{
    public interface ICustomerService
    {
        Task<ServiceResponse<List<GetCustomerDto>>> GetAllCustomers();
        Task<ServiceResponse<GetCustomerDto>> GetCustomerById(int id);
        Task<ServiceResponse<List<GetCustomerDto>>> AddCustomer(AddCustomerDto customer);
        Task<ServiceResponse<GetCustomerDto>> UpdateCustomer(UpdateCustomerDto customer,int id);
        Task<ServiceResponse<List<GetCustomerDto>>> DeleteCustomer(int id);
    }
}