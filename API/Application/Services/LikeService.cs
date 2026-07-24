using API.Domain.Constants;
using API.Domain.Enums;
using Blog_Project.Application.Interfaces;

namespace Blog_Project.Application.Services
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LikeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<LikeDetailsDto> CreateLikeAsync(int postId, int userId)
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
            var oldlike= await _unitOfWork.Repository<Like, int>().FirstOrDefaultAsync(l=>l.UserId==userId&&l.PostId==postId);
            if(oldlike != null)
            {
                return _mapper.Map<LikeDetailsDto>(oldlike);
            }
            var like = new Like();
            like.UserId = userId;
            like.PostId = postId;
            like.CreatedAt= DateTime.UtcNow;
            await _unitOfWork.Repository<Like, int>().AddAsync(like);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LikeDetailsDto>(like);
        }

        public async Task<bool> DeleteLikeAsync(int postId, int userId)
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
            var oldlike = await _unitOfWork.Repository<Like, int>().FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);
            if (oldlike == null)
            {
                return true;
            }
    
            _unitOfWork.Repository<Like, int>().Remove(oldlike);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetLikesAsync(int postId)
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
            var Likes = await _unitOfWork.Repository<Like, int>()
                .CountAsync(c => c.PostId == postId);
            return Likes;
        }
    }
}
