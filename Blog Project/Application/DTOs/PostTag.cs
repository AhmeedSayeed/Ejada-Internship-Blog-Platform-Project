namespace Blog_Project.Application.DTOs;

public record CreatePostTagDto(
    int PostId,
    int TagId
);
public record UpdatePostTagDto(
    int PostId,
    int OldTagId,
    int NewTagId
);