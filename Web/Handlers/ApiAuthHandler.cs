using Contract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Net.Http.Headers;
using Web.ApiClients;

namespace Web.Handlers
{
    public class ApiAuthHandler(IHttpContextAccessor httpContextAccessor,
        IServiceProvider serviceProvider) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var context = httpContextAccessor.HttpContext;

            if (context is null)
                return await base.SendAsync(request, cancellationToken);

            var accessToken = await context.GetTokenAsync("access_token");

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshToken = await context.GetTokenAsync("refresh_token");

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var authApiClient = serviceProvider.GetRequiredService<IAuthApiClient>();
                    var refreshResult = await authApiClient.RefreshTokenAsync(new RefreshTokenRequestDto { RefreshToken = refreshToken });

                    if (refreshResult.IsSuccess)
                    {
                        var authResult = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        if (authResult.Succeeded && authResult.Principal is not null)
                        {
                            var properties = authResult.Properties;
                            properties.UpdateTokenValue("access_token", refreshResult.Data!.AccessToken);
                            properties.UpdateTokenValue("refresh_token", refreshResult.Data!.RefreshToken);

                            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authResult.Principal, properties);

                            response.Dispose();
                            var clonedRequest = await CloneHttpRequestAsync(request);
                            clonedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshResult.Data.AccessToken);

                            return await base.SendAsync(clonedRequest, cancellationToken);
                        }
                    }
                    else
                    {
                        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                }
            }

            return response;
        }

        private async Task<HttpRequestMessage> CloneHttpRequestAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version
            };

            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            if (request.Content != null)
            {
                var ms = new MemoryStream();
                await request.Content.CopyToAsync(ms);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            return clone;
        }
    }
}
