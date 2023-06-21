using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Organisation_WebAPI.Dtos.Admin;
using Organisation_WebAPI.Dtos.ProductDto;

namespace Organisation_WebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, GetProductDto>();
            CreateMap<Admin, AdminRegisterDto>();
        }

        
    }
}