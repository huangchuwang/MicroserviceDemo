using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductService.Data;
using ProductService.Services;
using SharedKernel.Extensions;
using System.Text;

namespace Product
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // 1. 注册数据库
            builder.Services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. 注册服务
            builder.Services.AddScoped<IMyProductService, MyProductService>();

            // 3. 配置 JWT 验证
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            // 建议：增加空值检查，防止配置缺失导致启动报错
            var secretKey = jwtSettings["SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("JwtSettings:SecretKey", "JWT SecretKey is missing in appsettings.json");
            }

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
                });

            var app = builder.Build();

            // SharedKernel 全局异常处理
            app.UseGlobalExceptionHandler();

            //app.UseHttpsRedirection();

            // 4. 先认证，再授权
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}