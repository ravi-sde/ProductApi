using Microsoft.EntityFrameworkCore;
using ProductApi.Models;


namespace ProductApi.DAL
{
    public class ProductDbContext:DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
