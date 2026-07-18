namespace Blog_Project.Application.DTOs;

public record CreateFollowDto(
    int FollowerId,
    int FollowingId
);
public record FollowDetailsDto(
    int FollowerId,
    int FollowingId,
    DateTime CreatedAt
);