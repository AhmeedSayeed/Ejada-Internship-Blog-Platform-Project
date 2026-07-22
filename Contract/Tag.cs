using System;
using System.Collections.Generic;
using System.Text;

namespace Contract;

public record CreateTagDto(string Name);
public record UpdateTagDto(int Id, string Name);
public record TagDto(int Id, string Name);