namespace Blog_Project.Application.DTOs;

public record CreateUserDto(
    string UserName,
    string ?ProfileImageUrl,
    string ?Bio
);
public record UpdateUserDto(
    int  UserId,
    string UserName,
    string? ProfileImageUrl,
    string? Bio
);

public record UserDetailsDto(
    int UserId,
    string UserName,
    string? ProfileImageUrl,
    string? Bio,
    bool IsSuspended,
    DateTime CreatedAt

);