using System;
using System.Collections.Generic;
using System.Text;

namespace Contract;

public record CreateCategoryDto(string Name, string? Description);
public record UpdateCategoryDto(int Id, string Name, string? Description);
public record CategoryDto(int Id, string Name, string? Description);