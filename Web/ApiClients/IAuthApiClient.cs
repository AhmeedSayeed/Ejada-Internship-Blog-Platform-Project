using Contract;

namespace Web.ApiClients
{
    public interface IAuthApiClient
    {
        Task<ApiResponse> RegisterAsync(RegisterRequestDto request);
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request);
        Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<ApiResponse> LogoutAsync(RefreshTokenRequestDto request);
    }
}
