namespace API.Application.DTOs;

public record CreateCategoryDto(
    string Name,
    string? Description
);
public record UpdateCategoryDto(
    int Id,
    string Name,
    string? Description
);
public record CategoryDto(
    int Id,
    string Name,
    string? Description
);
