namespace Microservice.IdentityServer.DTOs
{
    public class AuthResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; } // JWT 字符串
    }
}
