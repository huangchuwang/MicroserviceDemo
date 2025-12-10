using System.Text.Json.Serialization;

namespace SharedKernel.Models
{

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        // 构造函数私有化，强制使用静态方法创建
        private ApiResponse() { }

        /// <summary>
        /// 成功的返回
        /// </summary>
        public static ApiResponse<T> Ok(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        /// <summary>
        /// 失败的返回
        /// </summary>
        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message
            };
        }
    }
}