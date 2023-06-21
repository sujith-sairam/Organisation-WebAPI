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

        public async Task<ServiceResponse<List<GetProductDto>>> AddProduct(AddProductDto newProduct)
        {
            var serviceResponse = new ServiceResponse<List<GetProductDto>>();
            var product = _mapper.Map<Product>(newProduct);

             _context.Products.Add(product);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Products.Select(c => _mapper.Map<GetProductDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetProductDto>>> DeleteProduct(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetProductDto>>();
            try {

            var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductID == id);
            if (product is null)
                throw new Exception($"Character with id '{id}' not found");
            
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Products.Select(c => _mapper.Map<GetProductDto>(c)).ToList();
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
                return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetProductDto>>> GetAllProducts()
        {
           
            var serviceResponse = new ServiceResponse<List<GetProductDto>>();
            var dbProducts = await _context.Products.ToListAsync();
            serviceResponse.Data = dbProducts.Select(c => _mapper.Map<GetProductDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProductDto>> GetProductById(int id)
        {
            
            var serviceResponse = new ServiceResponse<GetProductDto>();
            var dbProduct =  await _context.Products.FirstOrDefaultAsync(c => c.ProductID == id);
            serviceResponse.Data = _mapper.Map<GetProductDto>(dbProduct);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProductDto>> UpdateProduct(UpdateProductDto updateProduct,int id)
        {
            var serviceResponse = new ServiceResponse<GetProductDto>();
            try {
                var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductID == id);
                if (product is null)
                    throw new Exception($"Character with id '{id}' not found");
                
                
                product.ProductName = updateProduct.ProductName;
                product.ProductManagerName = updateProduct.ProductManagerName;
                product.ProductRevenue = updateProduct.ProductRevenue;
            

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetProductDto>(product);

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
  