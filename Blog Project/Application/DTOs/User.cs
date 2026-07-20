using Blog_Project.Domain.Constants;
using Blog_Project.Domain.Models;

namespace Blog_Project.Application.DTOs;

// ==========================================
// Authentication DTOs
// ==========================================

// Used for POST /api/auth/register
public record RegisterRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = AppRoles.Reader;
}

// Used for POST /api/auth/login
public record LoginRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

// Used for Responses from Login and Refresh-Token
public record AuthResponseDto
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}

// Used for POST /api/auth/refresh-token
public record RefreshTokenRequestDto
{
    public string RefreshToken { get; init; } = string.Empty;
}

// An Internal Wrapper for the result of an authentication operation (login, register, refresh token) 
public record AuthResult
{
    public bool IsSuccess { get; init; }
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
    public IEnumerable<string>? Errors { get; init; }
}

// ==========================================
// User Profile DTOs
// ==========================================

// Used for PUT /api/users/me
public record UpdateProfileDto
{
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? Bio { get; init; }
    public string? ProfileImageUrl { get; init; }
    public string? CurrentPassword { get; init; }
    public string? NewPassword { get; init; }
}

// Used for GET /api/users/me
public record MyProfileResponseDto
{
    public int UserId { get; init; }
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string? ProfileImageUrl { get; init; }
    public string? Bio { get; init; }
    public IEnumerable<PostDto>? Posts { get; init; } = new List<PostDto>();
    public DateTime CreatedAt { get; init; }
}

// Used for GET /api/users/{id}
public record PublicProfileResponseDto
{
    public int UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string? ProfileImageUrl { get; init; }
    public string? Bio { get; init; }
    public IEnumerable<PostDto>? Posts { get; init; } = new List<PostDto>();
}

// Used for Following and Followers endpoints
public record UserSummaryDto
{
    public int UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string? ProfileImageUrl { get; init; }
}