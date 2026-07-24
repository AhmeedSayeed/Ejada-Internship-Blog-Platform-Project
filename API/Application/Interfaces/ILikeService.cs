namespace Blog_Project.Application.Interfaces
{
    public interface ILikeService
    {
        Task<int> GetLikesAsync(int postId);
        Task<LikeDetailsDto> CreateLikeAsync(int postId, int userId);
        Task<bool> DeleteLikeAsync(int postId, int userId);
    }
}
