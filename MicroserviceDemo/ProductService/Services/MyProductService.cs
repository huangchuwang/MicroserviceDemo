using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Data.Entities;
using ProductService.DTOs;
using SharedKernel.Exceptions;

namespace ProductService.Services
{
    public class MyProductService : IMyProductService
    {
        private readonly ProductDbContext _context;

        public MyProductService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            // 简单映射，实际可用 AutoMapper
            return products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            }).ToList();
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
        {
            if (request.Price <= 0)
                throw new BusinessException("价格必须大于0");

            var product = new ProductItem
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductResponse { Id = product.Id, Name = product.Name, Price = product.Price };
        }
    }
}
