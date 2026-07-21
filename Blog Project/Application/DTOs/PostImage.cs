namespace Blog_Project.Application.DTOs;

public record PostImageDto(
    int PostId,
    IFormFile ImageFile
);
