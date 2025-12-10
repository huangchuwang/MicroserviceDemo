using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharedKernel.Exceptions;
using SharedKernel.Models;
using System.Net;
using System.Text.Json;

namespace SharedKernel.Middlewares
{

    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // 继续执行后续的中间件（Controller 等）
                await _next(context);
            }
            catch (Exception ex)
            {
                // 如果报错了，进入这里处理
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // 1. 默认状态码 500 (服务器内部错误)
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = ApiResponse<string>.Fail("Server Error");

            // 2. 如果是我们自定义的业务异常，状态码改为 200 或 400，并显示具体错误信息
            if (exception is BusinessException businessEx)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 或者使用 200 OK
                response.Message = businessEx.Message;
                _logger.LogWarning($"Business Logic Error: {businessEx.Message}");
            }
            else
            {
                // 如果是系统未知错误，记录详细日志，但在返回中隐藏堆栈信息
                _logger.LogError(exception, "System Unknown Error");
                response.Message = "An internal error occurred.";
            }

            // 3. 序列化并写入响应
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, jsonOptions);

            await context.Response.WriteAsync(json);
        }
    }
}