using Microservice.IdentityServer.Data;
using Microservice.IdentityServer.Data.Entities;
using Microservice.IdentityServer.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Microservice.IdentityServer.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // 1. 查询用户
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            // 2. 校验用户和密码 (这里简化了，实际请用 BCrypt.Verify)
            if (user == null || user.PasswordHash != request.Password)
            {
                // 直接抛出业务异常，SharedKernel 的中间件会捕获它并返回 400
                throw new BusinessException("用户名或密码错误");
            }

            // 3. 生成 Token
            var token = GenerateJwtToken(user);

            return new AuthResponse { UserId = user.Id, Username = user.Username, Token = token };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // 1. 检查是否存在
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                throw new BusinessException("用户名已存在");
            }

            // 2. 创建实体
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = request.Password // 实际请用 BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 3. 注册成功后直接返回 Token，让用户免登录
            var token = GenerateJwtToken(user);
            return new AuthResponse { UserId = user.Id, Username = user.Username, Token = token };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
