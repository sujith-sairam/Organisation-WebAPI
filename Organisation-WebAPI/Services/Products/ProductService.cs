using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Dtos.ProductDto;

namespace Organisation_WebAPI.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly OrganizationContext _context;
        
        public ProductService(OrganizationContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetProductDto>>> GetAllProducts()
        {
           
            var serviceResponse = new ServiceResponse<List<GetProductDto>>();
            var dbCharacters = await _context.Products.ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetProductDto>(c)).ToList();
            return serviceResponse;
        }
    }
}