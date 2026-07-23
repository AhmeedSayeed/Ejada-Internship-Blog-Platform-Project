
using API.Domain.Constants;
using API.Domain.Enums;
using Blog_Project.Application.Interfaces;

namespace Blog_Project.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public PostImgService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<CommentDetailsDto> CreateCommentAsync(CreateCommentDto commentDto, int userId)
        {
            var post = await _unitOfWork.Repository<Post>().GetByIdAsync(commentDto.PostId);
            if (post==null)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            if (commentDto.ParentCommentId.HasValue)
            {
                var parentComment = await _unitOfWork.Repository<Comment, int>()
                    .GetByIdAsync(commentDto.ParentCommentId.Value);

                if (parentComment == null)
                    throw new Exception(ErrorMessages.CommentNotFound);

                if (parentComment.PostId != commentDto.PostId)
                    throw new Exception(ErrorMessages.InvalidComment);
            }
            var comment = _mapper.Map<Comment>(commentDto);
            
            comment.UserId = userId;
            comment.CreatedAt = DateTime.UtcNow;
            comment.Status = CommentStatus.Flagged;
            await _unitOfWork.Repository<Comment, int>().AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CommentDetailsDto>(comment);
        }

        public Task DeleteCommentAsync(int CommentId, int userId)
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
            comment.Content = commentDto.Content;
            await _unitOfWork.Repository<Comment, int>().AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CommentDetailsDto>(comment);
        }

        public async Task<CommentDetailsDto> GetCommentsAsync(int postId)
        {
            var post = await _unitOfWork.Repository<Post>().GetByIdAsync(postId);
            if(post==null)
            {
                throw new Exception(ErrorMessages.PostNotFound);
            }
            var Comments = await _unitOfWork.Repository<Comment, int>()
                .Where(c => c.PostId == postId)
                .ToListAsync();
            return _mapper.Map<List<CommentDetailsDto>>(Comments);
        }

        public async Task<UpdateCommentStatusDto?> ApproveCommentAsync(int CommentId)
        {
            var comment = await _unitOfWork.Repository<Comment, int>()
               .FirstOrDefaultAsync(c => c.Id == commentDto.CommentId && c.PostId == commentDto.PostId);
            if (comment == null)
            {
                throw new Exception(ErrorMessages.CommentNotFound);
            }
            comment.Status = CommentStatus.Approved;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UpdateCommentStatusDto>(comment);
        }
        public async Task<UpdateCommentStatusDto?> RejectCommentAsync(int CommentId)
        {
            var comment = await _unitOfWork.Repository<Comment, int>()
                .FirstOrDefaultAsync(c => c.Id == commentDto.CommentId && c.PostId == commentDto.PostId);
            if (comment == null)
            {
                throw new Exception(ErrorMessages.CommentNotFound);
            }
            comment.Status = CommentStatus.Rejected;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<UpdateCommentStatusDto>(comment);
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
