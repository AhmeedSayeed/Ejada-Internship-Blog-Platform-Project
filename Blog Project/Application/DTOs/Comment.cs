namespace Blog_Project.Application.DTOs;

public record CreateCommentDto(
    string Content,
    int PostId,
    int UserId,
    int? ParentCommentId
);

public record UpdateCommentDto(
    int Id,
    string Content
 );

public record UpdateCommentStatusDto(
    string Status
 );

public record CommentDetailsDto(
    int Id,
    string Content,
    int PostId,
    int UserId,
    string UserName,
    string? UserProfileImageUrl,
    int? ParentCommentId,
    DateTime CreatedAt
);

