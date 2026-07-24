using Contract;
using System.Text.Json;

namespace Web.ApiClients
{
    public class AuthApiClient(HttpClient httpClient) : IAuthApiClient
    {
        public async Task<ApiResponse> RegisterAsync(RegisterRequestDto request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", request);

            if (response.IsSuccessStatusCode)
                return new ApiResponse { IsSuccess = true };

            var error = await HandleError<object>(response);
            return new ApiResponse { IsSuccess = false, ErrorMessage = error.ErrorMessage, Errors = error.Errors };
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                return new ApiResponse<AuthResponseDto> { IsSuccess = true, Data = data };
            }

            return await HandleError<AuthResponseDto>(response);
        }

        public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/refresh-token", request);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                return new ApiResponse<AuthResponseDto> { IsSuccess = true, Data = data };

            }

            return await HandleError<AuthResponseDto>(response);
        }

        public async Task<ApiResponse> LogoutAsync(RefreshTokenRequestDto request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/logout", request);

            if (response.IsSuccessStatusCode)
                return new ApiResponse { IsSuccess = true };

            var error = await HandleError<object>(response);
            return new ApiResponse { IsSuccess = false, ErrorMessage = error.ErrorMessage, Errors = error.Errors };
        }

        private async Task<ApiResponse<T>> HandleError<T>(HttpResponseMessage response)
        {
            try
            {
                var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    ErrorMessage = error?.Title ?? "Something went wrong.",
                    Errors = error?.Errors?.Any() == true ? error.Errors : new[] { $"HTTP {(int)response.StatusCode}" }
                };
            }
            catch (JsonException)
            {
                return new ApiResponse<T> { IsSuccess = false, ErrorMessage = $"HTTP {(int)response.StatusCode}", Errors = Array.Empty<string>() };
            }
        }
    }
}
