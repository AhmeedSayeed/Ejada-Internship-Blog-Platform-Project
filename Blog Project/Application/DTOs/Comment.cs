namespace Blog_Project.Application.DTOs;

public record CommentDto(
        int Id,
        string Content,
        int PostId,
        int UserId,
        string UserName,
        string? UserProfileImageUrl,
        DateTime CreatedAt
    );
public record CreateCommentDto(
    string Content,
    int PostId,
    int? ParentCommentId
);
public record UpdateCommentDto(
    string Content
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

