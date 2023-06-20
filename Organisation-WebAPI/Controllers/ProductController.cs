using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("GetAll")]
          public async Task<ActionResult<ServiceResponse<GetProductDto>>> Get()
        {
            return Ok(await _productService.GetAllProducts());
        }
    }
}