using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Organisation_WebAPI.Dtos.ProductDto;

namespace Organisation_WebAPI.Services.Products
{
    public interface IProductService
    {
        Task<ServiceResponse<List<GetProductDto>>> GetAllProducts();
        //Task<ServiceResponse<List<GetProductDto>>> GetAvailableProducts();
        Task<ServiceResponse<GetProductDto>> GetProductById(int id);
        Task<ServiceResponse<Dictionary<string,int>>> GetRevenue();
        Task<ServiceResponse<List<GetProductDto>>> AddProduct(AddProductDto newProduct);
        Task<ServiceResponse<GetProductDto>> UpdateProduct(UpdateProductDto product,int id);
        Task<ServiceResponse<List<GetProductDto>>> DeleteProduct(int id);
    }
}