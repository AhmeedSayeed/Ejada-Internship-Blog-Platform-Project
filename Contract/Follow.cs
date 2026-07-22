using System;
using System.Collections.Generic;
using System.Text;

namespace Contract;

public record CreateFollowDto(int FollowerId, int FollowingId);
public record FollowDetailsDto(int FollowerId, int FollowingId, DateTime CreatedAt);