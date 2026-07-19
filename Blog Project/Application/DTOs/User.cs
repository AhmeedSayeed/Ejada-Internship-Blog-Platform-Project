using Blog_Project.Domain.Models;

namespace Blog_Project.Application.DTOs;

// ==========================================
// Authentication DTOs
// ==========================================

// Used for POST /api/auth/register
public record RegisterRequestDto(
    string Email,
    string UserName,
    string Password
);

// Used for POST /api/auth/login
public record LoginRequestDto(
    string Email,
    string Password
);

// Used for Responses from Login and Refresh-Token
public record AuthResponseDto(
    string AccessToken,
    string RefreshToken
);

// Used for POST /api/auth/refresh-token
public record RefreshTokenRequestDto(
    string RefreshToken
);

// An Internal Wrapper for the result of an authentication operation (login, register, refresh token) 
public record AuthResult(
    bool IsSuccess,
    string? AccessToken,
    string? RefreshToken,
    IEnumerable<string>? Errors
);


// ==========================================
// User Profile DTOs
// ==========================================

// Used for PUT /api/users/me
public record UpdateProfileDto(
    string? UserName,
    string? Email,
    string? Bio,
    string? ProfileImageUrl,
    string? CurrentPassword,
    string? NewPassword
);

// Used for GET /api/users/me
public record MyProfileResponseDto(
    int UserId,
    string Email,
    string UserName,
    string? ProfileImageUrl,
    string? Bio,
    IEnumerable<PostDto>? Posts,
    DateTime CreatedAt
);

// Used for GET /api/users/{id}
public record PublicProfileResponseDto(
    int UserId,
    string UserName,
    string? ProfileImageUrl,
    string? Bio,
    IEnumerable<PostDto>? Posts
);
