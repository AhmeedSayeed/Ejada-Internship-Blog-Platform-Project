using Application.Interfaces;
using AutoMapper;
using Blog_Project.Application.DTOs;
using Blog_Project.Application.Interfaces;
using Blog_Project.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog_Project.Application.Services
{
    public class UserProfileService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper) : IUserProfileService
    {
        public async Task<Result<MyProfileResponseDto>> GetMyProfileAsync(int userId)
        {
            var user = await unitOfWork.Repository<ApplicationUser, int>()
                .GetByIdAsync(userId, includes: u => u.Posts.OrderByDescending(p => p.CreatedAt).Take(5));

            if (user is null)
                return Result<MyProfileResponseDto>.Failure(new Error("User.NotFound", "The user with the specified ID does not exist."));

            var dto = mapper.Map<MyProfileResponseDto>(user);

            return Result<MyProfileResponseDto>.Success(dto);
        }

        public async Task<Result<bool>> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Result<bool>.Failure(new Error("User.NotFound", "The user with the specified ID does not exist."));

            if(!string.IsNullOrEmpty(dto.UserName))
                user.UserName = dto.UserName;
            if(!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Bio))
                user.Bio = dto.Bio;
            if(!string.IsNullOrEmpty(dto.ProfileImageUrl))
                user.ProfileImageUrl = dto.ProfileImageUrl;

            if (!string.IsNullOrEmpty(dto.CurrentPassword) && !string.IsNullOrEmpty(dto.NewPassword))
            {
                var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                if(!result.Succeeded)
                    return Result<bool>.Failure(new Error("Password.ChangeFailed", 
                        result.Errors.FirstOrDefault()?.Description ?? "An unknown error occurred during password change."));
            }

            await userManager.UpdateAsync(user);

            return Result<bool>.Success(true);
        }

        public async Task<Result<PublicProfileResponseDto>> GetPublicProfileAsync(int targetUserId)
        {
            var user = await unitOfWork.Repository<ApplicationUser, int>()
                .GetByIdAsync(targetUserId, includes: u => u.Posts.OrderByDescending(p => p.CreatedAt).Take(5));

            if (user is null)
                return Result<PublicProfileResponseDto>.Failure(new Error("User.NotFound", "The user with the specified ID does not exist."));

            var dto = mapper.Map<PublicProfileResponseDto>(user);

            return Result<PublicProfileResponseDto>.Success(dto);
        }

        public async Task<Result<bool>> FollowAsync(int followerId, int targetUserId)
        {
            if(followerId == targetUserId)
                return Result<bool>.Failure(new Error("Follow.Invalid", "You cannot follow yourself."));

            var follower = await unitOfWork.Repository<ApplicationUser, int>().GetByIdAsync(followerId);
            if (follower is null)
                return Result<bool>.Failure(new Error("Follower.NotFound", "The follower user with the specified ID does not exist."));

            var targetUser = await unitOfWork.Repository<ApplicationUser, int>().GetByIdAsync(targetUserId);
            if (targetUser is null)
                return Result<bool>.Failure(new Error("TargetUser.NotFound", "The target user with the specified ID does not exist."));

            var existingFollow = await unitOfWork.Repository<Follow, int>().SingleOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == targetUserId);
            if (existingFollow is not null)
                return Result<bool>.Failure(new Error("Follow.AlreadyExists", "The user is already following the target user."));

            var follow = new Follow
            {
                FollowerId = followerId,
                FollowingId = targetUserId
            };

            await unitOfWork.Repository<Follow, int>().AddAsync(follow);
            await unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UnfollowAsync(int followerId, int targetUserId)
        {
            var follower = await unitOfWork.Repository<ApplicationUser, int>().GetByIdAsync(followerId);
            if (follower is null)
                return Result<bool>.Failure(new Error("Follower.NotFound", "The follower user with the specified ID does not exist."));

            var targetUser = await unitOfWork.Repository<ApplicationUser, int>().GetByIdAsync(targetUserId);
            if (targetUser is null)
                return Result<bool>.Failure(new Error("TargetUser.NotFound", "The target user with the specified ID does not exist."));

            var existingFollow = await unitOfWork.Repository<Follow, int>().SingleOrDefaultAsync(f => f.FollowerId == followerId && f.FollowingId == targetUserId);
            if (existingFollow is null)
                return Result<bool>.Failure(new Error("Follow.NotFound", "You are not following this user."));

            unitOfWork.Repository<Follow, int>().Remove(existingFollow);
            await unitOfWork.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<UserSummaryDto>>> GetFollowingAsync(int userId)
        {
            var following = await unitOfWork.Repository<Follow, int>()
                .FindAsync(f => f.FollowerId == userId, includes: f => f.FollowingUser);
            var dtoList = following.Select(f => mapper.Map<UserSummaryDto>(f.FollowingUser));
            return Result<IEnumerable<UserSummaryDto>>.Success(dtoList);
        }

        public async Task<Result<IEnumerable<UserSummaryDto>>> GetFollowersAsync(int targetUserId)
        {
            var followers = await unitOfWork.Repository<Follow, int>()
                .FindAsync(f => f.FollowingId == targetUserId, includes: f => f.Follower);
            var dtoList = followers.Select(f => mapper.Map<UserSummaryDto>(f.Follower));
            return Result<IEnumerable<UserSummaryDto>>.Success(dtoList);
        }
    }
}
