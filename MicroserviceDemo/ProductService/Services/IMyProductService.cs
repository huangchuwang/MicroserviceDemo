using ProductService.DTOs;

namespace ProductService.Services
{
    public interface IMyProductService
    {
        Task<List<ProductResponse>> GetAllAsync();
        Task<ProductResponse> CreateAsync(CreateProductRequest request);
    }
}
