using Blog_Project.Application.DTOs;

namespace Blog_Project.Application.Interfaces
{
    public interface IUserProfileService
    {
        Task<Result<MyProfileResponseDto>> GetMyProfileAsync(int userId);
        Task<Result<bool>> UpdateProfileAsync(int userId, UpdateProfileDto dto);
        Task<Result<PublicProfileResponseDto>> GetPublicProfileAsync(int targetUserId);
        Task<Result<bool>> FollowAsync(int followerId, int targetUserId);
        Task<Result<bool>> UnfollowAsync(int followerId, int targetUserId);
        Task<Result<IEnumerable<UserSummaryDto>>> GetFollowingAsync(int userId);
        Task<Result<IEnumerable<UserSummaryDto>>> GetFollowersAsync(int targetUserId);
    }
}
