using Blog_Project.Application.DTOs;
using Blog_Project.Domain.Models;

namespace Blog_Project.Application.Interfaces
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
