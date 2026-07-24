using API.Domain.Constants;
using API.Domain.Enums;
using Blog_Project.Application.Interfaces;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Blog_Project.Application.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<double> AvgRatingAsync(int postId)
        {
            var post = await _unitOfWork.Repository<Post, int>().GetByIdAsync(postId);
            if (post == null)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            if (post.Status != PostStatus.Approved)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            return post.AvgRating;
        }

        public  async Task<RatingDetailsDto> CreateRatingAsync(int postId, CreateRatingDto ratingDto, int userId)
        {
            var post = await _unitOfWork.Repository<Post, int>().GetByIdAsync(postId);
            if (post == null)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            if (post.Status != PostStatus.Approved)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            if(post.AuthorId == userId)
            {
                throw new Exception(ErrorMessages.Unauthorized);
            }
            var existingRating = await _unitOfWork.Repository<Rating, int>()
                .FirstOrDefaultAsync(r =>
                    r.PostId == postId &&
                    r.UserId == userId);
            if(existingRating!=null)
            {
                existingRating.Score = ratingDto.Score;
                _unitOfWork.Repository<Rating, int>().Update(existingRating);
                await _unitOfWork.SaveChangesAsync();
                post.AvgRating = await _unitOfWork.Repository<Rating, int>()
                .Query()
                .Where(r => r.PostId == postId)
                .AverageAsync(r => r.Score);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<RatingDetailsDto>(existingRating);
            }
            var rate = new Rating
            {
                Score = ratingDto.Score,
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Repository<Rating, int>().AddAsync(rate);
            await _unitOfWork.SaveChangesAsync();
            post.AvgRating = await _unitOfWork.Repository<Rating, int>()
            .Query()
            .Where(r => r.PostId == postId)
            .AverageAsync(r => r.Score);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RatingDetailsDto>(rate);
        }

        public async Task<List<RatingDetailsDto>> GetAuthorRatingsAsync(int userId)
        {
            var ratings = await _unitOfWork.Repository<Rating, int>()
                .Query()
                .Include(r => r.Post)
                .Include(r => r.User)
                .Where(r => r.Post.AuthorId == userId)
                .ToListAsync();
            return _mapper.Map<List<RatingDetailsDto>>(ratings);
        }

        public async Task<RatingDetailsDto> UpdateRatingAsync(int postId, UpdateRatingDto ratingDto, int userId)
        {
            var post = await _unitOfWork.Repository<Post, int>().GetByIdAsync(postId);
            if (post == null)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            if (post.Status != PostStatus.Approved)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            var rate = await _unitOfWork.Repository<Rating, int>()
                .FirstOrDefaultAsync(r=>r.UserId ==userId&&r.PostId ==postId);
            if(rate == null)
            {
                throw new Exception(ErrorMessages.RateNotFound);
            }
            rate.Score = ratingDto.Score;
             _unitOfWork.Repository<Rating, int>().Update(rate);
            await _unitOfWork.SaveChangesAsync();
            post.AvgRating = await _unitOfWork.Repository<Rating, int>()
            .Query()
            .Where(r => r.PostId == postId)
            .AverageAsync(r => r.Score);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<RatingDetailsDto>(rate);
        }
    }
}
