using AutoMapper;
using Contract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Web.ApiClients;
using Web.ViewModel;

namespace Web.Controllers
{
    public class AccountController(IAuthApiClient authApiClient, IMapper mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var dto = mapper.Map<RegisterViewModel, RegisterRequestDto>(model);

            var response = await authApiClient.RegisterAsync(dto);

            if (response.IsSuccess)
                return RedirectToAction("Login", "Account");

            if (response.Errors is not null && response.Errors.Any())
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, response.ErrorMessage ?? "Registration failed.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (User.Identity is not null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var dto = mapper.Map<LoginViewModel, LoginRequestDto>(model);

            var response = await authApiClient.LoginAsync(dto);

            if (!response.IsSuccess)
            {
                if (response.Errors is not null && response.Errors.Any())
                {
                    foreach (var error in response.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.ErrorMessage ?? "Login failed.");
                }
                return View(model);
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(response.Data!.AccessToken);

            var identity = new ClaimsIdentity(jwtToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties();
            authProperties.StoreTokens(new List<AuthenticationToken>
            {
                new AuthenticationToken { Name = "access_token", Value = response.Data!.AccessToken },
                new AuthenticationToken { Name = "refresh_token", Value = response.Data!.RefreshToken }
            });

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await authApiClient.LogoutAsync(new RefreshTokenRequestDto { RefreshToken = refreshToken });
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}