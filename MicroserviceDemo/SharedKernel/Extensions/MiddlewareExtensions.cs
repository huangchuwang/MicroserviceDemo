using Microsoft.AspNetCore.Builder;

namespace SharedKernel.Extensions
{

    public static class MiddlewareExtensions
    {
        /// <summary>
        /// 启用全局异常捕获
        /// </summary>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<Middlewares.GlobalExceptionMiddleware>();
        }
    }
}