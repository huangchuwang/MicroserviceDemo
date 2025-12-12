using Microservice.IdentityServer.DTOs;

namespace Microservice.IdentityServer.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
