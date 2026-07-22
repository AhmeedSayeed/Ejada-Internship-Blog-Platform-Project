namespace API.Application.DTOs;

public record PostImageDto(
    int PostId,
    IFormFile ImageFile
);
public record GetPostImageDto(
    int Id,
    string ImageUrl
);