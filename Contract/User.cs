using System;
using System.Collections.Generic;
using System.Text;

namespace Contract;

public record RegisterRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = "Reader";
}

public record LoginRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public record AuthResponseDto
{
    public string AccessToken { get; init; } = string.Empty;
    public string RefreshToken { get; init; } = string.Empty;
}

public record RefreshTokenRequestDto
{
    public string RefreshToken { get; init; } = string.Empty;
}

public record UpdateProfileDto
{
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public string? Bio { get; init; }
    public string? ProfileImageUrl { get; init; }
    public string? CurrentPassword { get; init; }
    public string? NewPassword { get; init; }
}

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

public record PublicProfileResponseDto
{
    public int UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string? ProfileImageUrl { get; init; }
    public string? Bio { get; init; }
    public IEnumerable<PostDto>? Posts { get; init; } = new List<PostDto>();
}

public record ProfileImageResponseDto
{
    public string Url { get; init; }
}

public record UserSummaryDto
{
    public int UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string? ProfileImageUrl { get; init; }
}