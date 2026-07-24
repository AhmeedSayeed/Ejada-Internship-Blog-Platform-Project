namespace Blog_Project.Application.Interfaces
{
    public interface IRatingService
    {
        Task<RatingDetailsDto> CreateRatingAsync(int postId, CreateRatingDto ratingDto, int userId);
        Task<RatingDetailsDto> UpdateRatingAsync(int postId, UpdateRatingDto ratingDto, int userId);
        Task<double> AvgRatingAsync(int postId);
        Task<List<RatingDetailsDto>> GetAuthorRatingsAsync(int userId);
    }
}
