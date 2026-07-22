using API.Application.DTOs;
using API.Domain.Models;

namespace API.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterRequestDto registerRequest);
        Task<AuthResult> LoginAsync(LoginRequestDto loginRequest);
        Task<AuthResult> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(string refreshToken);
        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
        RefreshToken GenerateRefreshToken(int userId);
    }
}
