namespace API.Application.DTOs;
public record CreateLikeDto(
    int PostId,
    int UserId
);
//When Removing likes its useless.
//public record UpdateLikeDto(
//    int Id
//);
public record LikeDetailsDto(
    int Id,
    int PostId,
    int UserId,
    DateTime CreatedAt
);
