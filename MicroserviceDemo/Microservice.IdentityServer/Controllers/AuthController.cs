using Microservice.IdentityServer.DTOs;
using Microservice.IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Models; // 引用通用返回模型

namespace Identity.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ApiResponse<AuthResponse>> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return ApiResponse<AuthResponse>.Ok(result, "登录成功");
        }

        [HttpPost("register")]
        public async Task<ApiResponse<AuthResponse>> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            return ApiResponse<AuthResponse>.Ok(result, "注册成功");
        }
    }
}