using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.Dtos.ProductDto;
using Organisation_WebAPI.Services.Products;

namespace Organisation_WebAPI.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // Retrieves all products from the database
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> GetProducts()
        {
            return Ok(await _productService.GetAllProducts());
        }

        // Retrieves a product from the database based on the provided ID
        [HttpGet("GetProductById")]
        
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> GetProduct(int id)
        {
            return Ok(await _productService.GetProductById(id));
        }

      

        //Retrieves all product revenues with product names
        [HttpGet("GetProductRevenue")]
        public async Task<ActionResult<ServiceResponse<int>>> GetProductRevenue()
        {
            var serviceResponse = await _productService.GetRevenue();
            return Ok(serviceResponse);
        }

        // Adds a new Product to the database
        [HttpPost("CreateProduct")]
        [AllowAnonymous]

        public async Task<ActionResult<ServiceResponse<GetProductDto>>> AddProduct(AddProductDto newProduct)
        {
            return Ok(await _productService.AddProduct(newProduct));
        }

        // Updates a product in the database based on the provided ID
        [HttpPut("UpdateProduct")]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> UpdateProduct(UpdateProductDto updatedProduct,int id){
            return Ok(await _productService.UpdateProduct(updatedProduct,id));
        }
        
        // Deletes a product from the database based on the provided ID
        [HttpDelete("DeleteProduct")]
        public async Task<ActionResult<ServiceResponse<GetProductDto>>> DeleteProduct(int id){
            return Ok(await _productService.DeleteProduct(id));
        }
    }
}