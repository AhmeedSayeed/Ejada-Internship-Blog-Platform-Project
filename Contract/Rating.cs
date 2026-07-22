using System;
using System.Collections.Generic;
using System.Text;

namespace Contract;

public record CreateRatingDto(int Score, int PostId, int UserId);
public record UpdateRatingDto(int Id, int Score);
public record RatingDetailsDto(
    int Id, int Score, int PostId, int UserId,
    string UserName, string? UserProfileImageUrl, DateTime CreatedAt
);