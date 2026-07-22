namespace API.Application.DTOs;

public record CreatePostDto(
    string Title,
    string Content,
    int? CategoryId
);

public record UpdatePostDto(
    int PostId,
    string Title,
    string Content,
    int? CategoryId
);

public record UpdatePostStatusDto(
    string Status
);

public record PostDto(
    int Id,
    string Title,
    string Author,
    string Content,
    string? Category,
    double AvgRating,
    int ViewCount,
    DateTime LastUpdateAt
);

public record PostDetailsDto(
    int Id,
    string Title,
    string Content,
    int? CategoryId,
    string? CategoryName,
    int AuthorId,
    string AuthorUserName,
    string AuthorEmail,
    string? AuthorProfileImageUrl,
    int? ReviewedByAdminId,
    string? ReviewedByAdminUserName,
    string? ReviewedByAdminEmail,
    string? ReviewedByAdminProfileImageUrl,
    int ViewCount,
    double AvgRating,
    int RatingCount,
    DateTime? SubmittedAt,
    DateTime? PublishedAt,
    DateTime CreatedAt,
    DateTime LastUpdatedAt
);


