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
        private readonly IMapper _mapper; // Provides object-object mapping
        private readonly OrganizationContext _context;  // Represents the database context
        
        public ProductService(OrganizationContext context,IMapper mapper)
        {
            _context = context; // Injects the OrganizationContext instance
            _mapper = mapper; // Injects the IMapper instance
        }

        // Adds a new product to the database
        public async Task<ServiceResponse<List<GetProductDto>>> AddProduct(AddProductDto newProduct)
        {
            var serviceResponse = new ServiceResponse<List<GetProductDto>>();
            var product = _mapper.Map<Product>(newProduct);
             _context.Products.Add(product);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Products.Select(c => _mapper.Map<GetProductDto>(c)).ToListAsync();
            return serviceResponse;
        }

        // Deletes a product from the database based on the provided ID
        public async Task<ServiceResponse<List<GetProductDto>>> DeleteProduct(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetProductDto>>();
            try {

            var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductID == id);
            if (product is null)
                throw new Exception($"Product with id '{id}' not found");
           
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

        // Retrieves all product from the database
        public async Task<ServiceResponse<List<GetProductDto>>> GetAllProducts()
        {
           
            var serviceResponse = new ServiceResponse<List<GetProductDto>>();
            var dbProducts = await _context.Products.ToListAsync();
            serviceResponse.Data = dbProducts.Select(c => _mapper.Map<GetProductDto>(c)).ToList();
            return serviceResponse;
        }

        //Retrieves a product from the database with Id
        public async Task<ServiceResponse<GetProductDto>> GetProductById(int id)
        {
            var serviceResponse = new ServiceResponse<GetProductDto>();
            try
            {
                var dbProduct =  await _context.Products.FirstOrDefaultAsync(c => c.ProductID == id);
                serviceResponse.Data = _mapper.Map<GetProductDto>(dbProduct);
                if (dbProduct is null)
                    throw new Exception($"Product with id '{id}' not found");

                return serviceResponse;
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        // Retrieves the count of product in the database
        

         // Retrieves the revenue of products in the database
        public async Task<ServiceResponse<Dictionary<string,int>>> GetRevenue(){

                var serviceResponse = new ServiceResponse<Dictionary<string,int>>();
                var revenueDictionary = new Dictionary<string, int>();

                // Query the database to retrieve the products and their revenues
                var products = await _context.Products.ToListAsync();

                // Iterate over the products and add the name and revenue to the dictionary
                foreach (var product in products)
                {
                    revenueDictionary.Add(product.ProductName!, product.ProductRevenue);
                }

                serviceResponse.Data = revenueDictionary;
                return serviceResponse;

        }

         // Updates a product in the database based on the provided ID
        public async Task<ServiceResponse<GetProductDto>> UpdateProduct(UpdateProductDto updateProduct,int id)
        {
            var serviceResponse = new ServiceResponse<GetProductDto>();
            try {
                var product = await _context.Products.FirstOrDefaultAsync(c => c.ProductID == id); //finds the product in the database
                if (product is null)
                    throw new Exception($"Product with id '{id}' not found");
                
                
                product.ProductName = updateProduct.ProductName;
                product.ProductRevenue = updateProduct.ProductRevenue;
            

                await _context.SaveChangesAsync(); //save changes in the database
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
  