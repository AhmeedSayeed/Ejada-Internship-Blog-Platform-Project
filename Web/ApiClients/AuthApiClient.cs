using Contract;

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
                var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
                return errorResult ?? new ApiResponse<T> { IsSuccess = false, ErrorMessage = "Unknown error occurred." };
            }
            catch
            {
                return new ApiResponse<T> { IsSuccess = false, ErrorMessage = $"HTTP {response.StatusCode}" };
            }
        }
    }
}
