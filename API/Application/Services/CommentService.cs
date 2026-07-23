
using API.Domain.Constants;
using API.Domain.Enums;
using Blog_Project.Application.Interfaces;

namespace Blog_Project.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<CommentDetailsDto> CreateCommentAsync(int postId,CreateCommentDto commentDto, int userId)
        {
            var post = await _unitOfWork.Repository<Post,int>().GetByIdAsync(postId);
            if (post==null)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            if(post.Status != PostStatus.Approved)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            if (commentDto.ParentCommentId.HasValue)
            {
                var parentComment = await _unitOfWork.Repository<Comment, int>()
                    .GetByIdAsync(commentDto.ParentCommentId.Value);

                if (parentComment == null)
                    throw new Exception(ErrorMessages.CommentNotFound);

                if (parentComment.PostId != postId)
                    throw new Exception(ErrorMessages.InvalidComment);
            }
            var comment = _mapper.Map<Comment>(commentDto);
            
            comment.UserId = userId;
            comment.PostId = postId;
            comment.CreatedAt = DateTime.UtcNow;
            comment.Status = CommentStatus.PendingApproval;
            await _unitOfWork.Repository<Comment, int>().AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            var createdComment = await _unitOfWork.Repository<Comment, int>()
            .GetByIdAsync(
                comment.Id,
                includes: c => c.User
            );
            var c= createdComment.User;

            return _mapper.Map<CommentDetailsDto>(createdComment);
        }

        public async Task<bool> DeleteCommentAsync(int CommentId, int userId)
        {
            var comment = await _unitOfWork.Repository<Comment, int>()
                .FirstOrDefaultAsync(c => c.Id == CommentId);
            if (comment == null)
            {
                throw new Exception(ErrorMessages.CommentNotFound);
            }
            if (comment.UserId != userId)
            {
                throw new Exception(ErrorMessages.Unauthorized);
            }
            comment.Status = CommentStatus.Deleted;
             //_unitOfWork.Repository<Comment, int>().Remove(comment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<CommentDetailsDto>> GetCommentsAsync(int postId)
        {
            var post = await _unitOfWork.Repository<Post,int>().GetByIdAsync(postId);
            if(post==null)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            if (post.Status != PostStatus.Approved)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            var Comments = await _unitOfWork.Repository<Comment, int>()
                .FindAsync(c => c.PostId == postId&&c.Status==CommentStatus.Approved);
            return _mapper.Map<List<CommentDetailsDto>>(Comments);
        }

        public async Task<CommentDetailsDto?> FlagCommentAsync(int CommentId)
        {
            var comment = await _unitOfWork.Repository<Comment, int>()
               .FirstOrDefaultAsync(c => c.Id == CommentId);
            if (comment == null)
            {
                throw new Exception(ErrorMessages.CommentNotFound);
            }
            comment.Status = CommentStatus.Flagged;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CommentDetailsDto>(comment);
        }
        public async Task<CommentDetailsDto?> ApproveCommentAsync(int CommentId)
        {
            var comment = await _unitOfWork.Repository<Comment, int>()
               .FirstOrDefaultAsync(c => c.Id == CommentId );
            if (comment == null)
            {
                throw new Exception(ErrorMessages.CommentNotFound);
            }
            comment.Status = CommentStatus.Approved;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CommentDetailsDto>(comment);
        }
        public async Task<CommentDetailsDto?> RejectCommentAsync(int CommentId)
        {
            var comment = await _unitOfWork.Repository<Comment, int>()
                .FirstOrDefaultAsync(c => c.Id == CommentId);
            if (comment == null)
            {
                throw new Exception(ErrorMessages.CommentNotFound);
            }
            comment.Status = CommentStatus.Rejected;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CommentDetailsDto>(comment);
        }
        

        public async Task<CommentDetailsDto?> UpdateCommentAsync(UpdateCommentDto commentDto, int userId)
        {
            var comment = await _unitOfWork.Repository<Comment,int>()
                .FirstOrDefaultAsync(c=> c.Id ==commentDto.CommentId&&c.PostId==commentDto.PostId);
            if (comment==null)
            {
                throw new Exception(ErrorMessages.CommentNotFound);
            }
            if(comment.UserId != userId)
            {
                throw new Exception(ErrorMessages.Unauthorized);
            }
            comment.Content = commentDto.Content;
            await _unitOfWork.Repository<Comment, int>().AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CommentDetailsDto>(comment);
        }
    }
}
