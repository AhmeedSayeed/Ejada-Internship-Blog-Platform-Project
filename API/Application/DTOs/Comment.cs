namespace API.Application.DTOs;

public record CreateCommentDto(
    string Content,
    int? ParentCommentId
);

public record UpdateCommentDto(
    int CommentId,
    int PostId,
    string Content
 );

public record UpdateCommentStatusDto(
    int CommentId,
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

