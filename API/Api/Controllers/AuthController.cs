using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        // POST api/<AuthController>/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            if (!AppRoles.SelfRegisterable.Contains(dto.Role))
                return BadRequest(new { Message = "Invalid role for self-registration." });

            var result = await authService.RegisterAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(new { Message = "User registered successfully." });
        }

        // POST api/<AuthController>/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var result = await authService.LoginAsync(dto);

            if (!result.IsSuccess)
                return Unauthorized(result.Errors);

            return Ok(new AuthResponseDto { AccessToken = result.AccessToken!, RefreshToken = result.RefreshToken! });
        }

        // POST api/<AuthController>/refresh-token
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto dto)
        {
            var result = await authService.RefreshTokenAsync(dto.RefreshToken);

            if (!result.IsSuccess)
                return Unauthorized(result.Errors);

            return Ok(new AuthResponseDto { AccessToken = result.AccessToken!, RefreshToken = result.RefreshToken! });
        }

        // POST api/<AuthController>/logout
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(RefreshTokenRequestDto dto)
        {
            var result = await authService.LogoutAsync(dto.RefreshToken);

            if (!result)
                return BadRequest(new { Message = "Invalid Refresh Token" });

            return NoContent();
        }
    }
}
