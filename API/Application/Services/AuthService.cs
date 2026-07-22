using Application.Interfaces;
using AutoMapper;
using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequestDto registerRequest)
        {
            var user = _mapper.Map<ApplicationUser>(registerRequest);
            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return new AuthResult { IsSuccess = false, AccessToken = null, RefreshToken = null, Errors = errors };
            }

            await _userManager.AddToRoleAsync(user, "Reader");

            return new AuthResult { IsSuccess = true, AccessToken = null, RefreshToken = null, Errors = null };
        }

        public async Task<AuthResult> LoginAsync(LoginRequestDto loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
                return new AuthResult { IsSuccess = false, AccessToken = null, RefreshToken = null, Errors = new List<string> { "Invalid email or password." } };

            var isValidPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!isValidPassword)
                return new AuthResult { IsSuccess = false, AccessToken = null, RefreshToken = null, Errors = new List<string> { "Invalid email or password." } };

            var accessToken = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken(user.Id);

            await _unitOfWork.Repository<RefreshToken, int>().AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResult { IsSuccess = true, AccessToken = accessToken, RefreshToken = refreshToken.Token, Errors = null };
        }

        public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
        {
            var oldToken = await _unitOfWork.Repository<RefreshToken, int>()
                .SingleOrDefaultAsync(rt => rt.Token == refreshToken, includes: rt => rt.User);

            if (oldToken is null || !oldToken.IsActive)
                return new AuthResult { IsSuccess = false, AccessToken = null, RefreshToken = null, Errors = new List<string> { "Invalid refresh token." } };

            await _unitOfWork.BeginTransactionAsync();

            oldToken.RevokedOn = DateTime.UtcNow;
            _unitOfWork.Repository<RefreshToken, int>().Update(oldToken);

            var newAccessToken = await GenerateJwtTokenAsync(oldToken.User);
            var newRefreshToken = GenerateRefreshToken(oldToken.UserId);
            await _unitOfWork.Repository<RefreshToken, int>().AddAsync(newRefreshToken);

            await _unitOfWork.CommitTransactionAsync();

            return new AuthResult { IsSuccess = true, AccessToken = newAccessToken, RefreshToken = newRefreshToken.Token, Errors = null };
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var oldToken = await _unitOfWork.Repository<RefreshToken, int>()
                .SingleOrDefaultAsync(rt => rt.Token == refreshToken);

            if (oldToken is null || !oldToken.IsActive)
                return false;

            oldToken.RevokedOn = DateTime.UtcNow;
            _unitOfWork.Repository<RefreshToken, int>().Update(oldToken);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:DurationInMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(int userId)
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    ExpiresOn = DateTime.UtcNow.AddDays(7),
                    CreatedOn = DateTime.UtcNow,
                    UserId = userId
                };
            }
        }
    }
}