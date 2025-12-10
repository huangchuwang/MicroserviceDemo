using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.DTOs;
using ProductService.Services;
using SharedKernel.Models;

namespace Product.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMyProductService _service;

        public ProductController(IMyProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ApiResponse<List<ProductResponse>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return ApiResponse<List<ProductResponse>>.Ok(data);
        }

        [HttpPost]
        [Authorize] // <--- 关键点：这个接口需要携带 Token 才能访问
        public async Task<ApiResponse<ProductResponse>> Create(CreateProductRequest request)
        {
            var data = await _service.CreateAsync(request);
            return ApiResponse<ProductResponse>.Ok(data);
        }
    }
}