using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.DAL;
using ProductApi.Models;
using ProductApi.Service.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Service.ServiceImplementation
{
    public class ProductService : IProductService
    {

        private readonly ProductDbContext _dbContext;

        public ProductService(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> DeleteProduct(int Id)
        {
            var products = await _dbContext.Products.FindAsync(Id);

            if (products != null)
            {
                _dbContext.Products.Remove(products);
                await _dbContext.SaveChangesAsync();
            }
            return products;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            IEnumerable<Product> lstProducts = null;
            lstProducts = await _dbContext.Products.ToListAsync(); 
          
            return lstProducts;
        }

        public async Task<Product> GetProductById(int Id)
        {
            var product = await _dbContext.Products.FindAsync(Id);

            if (product == null) 
                return null;

            return product;
        }

        public async Task<Product> PostProduct([FromBody] Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateProduct(int Id, Product product)
        {
            product.Id = Id;

            _dbContext.Entry(product).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return product;
        }

        private bool ProductExists(int id)
        {
            return _dbContext.Products.Any(e => e.Id == id);
        }
    }
}
