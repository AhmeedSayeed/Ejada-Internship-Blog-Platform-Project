namespace API.Application.DTOs;

public record PostImageDto(
    int PostId,
    IFormFile ImageFile
);
