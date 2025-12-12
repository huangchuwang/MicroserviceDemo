using Microsoft.EntityFrameworkCore;
using ProductService.Data.Entities;
using System.Collections.Generic;

namespace ProductService.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }
        public DbSet<ProductItem> Products { get; set; }
    }
}
