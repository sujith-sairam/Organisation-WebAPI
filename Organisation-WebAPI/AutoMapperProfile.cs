using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Organisation_WebAPI.Dtos.Admin;
using Organisation_WebAPI.Dtos.ProductDto;
using Organisation_WebAPI.Dtos.CustomerDto;
using Organisation_WebAPI.Dtos.DepartmentDto;
using Organisation_WebAPI.Dtos.EmployeeDto;
using Organisation_WebAPI.Dtos.EmployeeTaskDto;
using Organisation_WebAPI.Dtos.ManagerDto;

namespace Organisation_WebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, GetProductDto>();
            CreateMap<AddProductDto,Product>();
            CreateMap<UpdateProductDto,Product>();

            CreateMap<Customer, GetCustomerDto>();
            CreateMap<AddCustomerDto,Customer>();
            CreateMap<UpdateCustomerDto,Customer>();

            CreateMap<Department, GetDepartmentDto>();
            CreateMap<AddDepartmentDto,Department>();
            CreateMap<UpdateDepartmentDto,Department>();

            CreateMap<Employee, GetEmployeeDto>();
            CreateMap<AddEmployeeDto,Employee>();
            CreateMap<UpdateEmployeeDto,Employee>();

            CreateMap<Manager, GetManagerDto>();
            CreateMap<AddManagerDto,Manager>();
            CreateMap<UpdateManagerDto,Manager>();

            CreateMap<EmployeeTask, GetEmployeeTaskDto>();
            CreateMap<AddEmployeeTaskDto,EmployeeTask>();
            CreateMap<UpdateEmployeeTaskDto,EmployeeTask>();
            CreateMap<UpdateEmployeeTaskStatusDto,EmployeeTask>();

        }

        
    }
}