using System;
using System.Collections.Generic;
using System.Text;

namespace Contract;

public record CreateLikeDto(int PostId, int UserId);
public record LikeDetailsDto(int Id, int PostId, int UserId, DateTime CreatedAt);