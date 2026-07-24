namespace API.Application.DTOs;

public record CreateRatingDto(
    int Score

);
public record UpdateRatingDto(
    int Score
);
public record RatingDetailsDto(
    int Id,
    int Score,
    int PostId,
    int UserId,
    string UserName,
    string? UserProfileImageUrl,
    DateTime CreatedAt
);
