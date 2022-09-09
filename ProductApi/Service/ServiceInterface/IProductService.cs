using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Service.ServiceInterface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int Id);
        Task<Product> DeleteProduct(int Id);
        Task<Product> UpdateProduct(int id, Product product);
        Task<Product> PostProduct([FromBody] Product product);

    }
}
