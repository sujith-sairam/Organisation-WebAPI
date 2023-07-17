using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Organisation_WebAPI.Dtos.Admin;
using Organisation_WebAPI.Dtos.ProductDto;
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
            CreateMap<User, GetUserDto>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

            CreateMap<Product, GetProductDto>();
            CreateMap<AddProductDto,Product>();
            CreateMap<UpdateProductDto,Product>();

            CreateMap<Department, GetDepartmentDto>();
            CreateMap<AddDepartmentDto,Department>();
            CreateMap<UpdateDepartmentDto,Department>();

            CreateMap<Employee, GetEmployeeDto>()
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.ManagerName : null));

            CreateMap<Employee, GetEmployeeDto>();
            CreateMap<AddEmployeeDto,Employee>();
            CreateMap<UpdateEmployeeDto,Employee>();

            CreateMap<Manager, GetManagerDto>();
            CreateMap<AddManagerDto,Manager>();
            CreateMap<UpdateManagerDto,Manager>();
            CreateMap<Manager, GetEmployeesAndManagerDto>()
            .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => src.ManagerId))
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.ManagerName))
            .ForMember(dest => dest.ManagerSalary, opt => opt.MapFrom(src => src.ManagerSalary))
            .ForMember(dest => dest.ManagerAge, opt => opt.MapFrom(src => src.ManagerAge));

            CreateMap<EmployeeTask, GetEmployeeTaskDto>();
            CreateMap<AddEmployeeTaskDto,EmployeeTask>();
            CreateMap<UpdateEmployeeTaskDto,EmployeeTask>();
            CreateMap<UpdateEmployeeTaskStatusDto,EmployeeTask>();

        }

        
    }
}