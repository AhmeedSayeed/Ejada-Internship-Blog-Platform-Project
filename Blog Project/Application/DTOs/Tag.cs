namespace Blog_Project.Application.DTOs;

public record CreateTagDto(
    string Name
);

public record UpdateTagDto(
    int Id,
    string Name
);

public record TagDto(
    int Id,
    string Name
);
