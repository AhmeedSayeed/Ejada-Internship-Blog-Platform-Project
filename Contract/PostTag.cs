using System;
using System.Collections.Generic;
using System.Text;

namespace Contract;

public record CreatePostTagDto(int PostId, int TagId);
public record UpdatePostTagDto(int PostId, int OldTagId, int NewTagId);