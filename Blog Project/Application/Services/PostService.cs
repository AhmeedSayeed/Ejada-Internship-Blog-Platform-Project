using Blog_Project.Domain.Enums;
using Blog_Project.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog_Project.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PostDto> CreatePostAsync(CreatePostDto postDto, int authorId)
        {
            var post = _mapper.Map<Post>(postDto);

            // Validation
            var author = await _userManager.FindByIdAsync(authorId.ToString());
            if (author == null)
                throw new Exception("Author not found.");

            if (postDto.CategoryId.HasValue)
            {
                var category = await _unitOfWork.Repository<Category, int>()
                    .GetByIdAsync(postDto.CategoryId.Value);

                if (category == null)
                    throw new Exception("Category not found.");
            }

            post.AuthorId = authorId;
            post.CreatedAt = DateTime.UtcNow;
            post.LastUpdatedAt = DateTime.UtcNow;
            post.Status = PostStatus.Draft;
            post.SubmittedAt = null;

            var value = await _unitOfWork.Repository<Post, int>()
                .AddAsync(post);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(value);
        }
        public async Task<PostDto?> UpdatePostAsync(UpdatePostDto postDto, int userId)
        {
            if (postDto.CategoryId.HasValue)
            {
                var category = await _unitOfWork.Repository<Category, int>()
                    .GetByIdAsync(postDto.CategoryId.Value);

                if (category == null)
                    throw new Exception("Category not found.");
            }

            var post = await _unitOfWork.Repository<Post, int>()
                .GetByIdAsync(postDto.PostId);

            if (post == null)
                throw new Exception("Post not found.");

            if (post.AuthorId != userId)
                throw new Exception("You are not authorized to update this post.");

            _mapper.Map(postDto, post);

            post.LastUpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Post, int>().Update(post);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(post);
        }

        public async Task<bool> DeletePostAsync(int postId, int userId)
        {
            var post = await _unitOfWork.Repository<Post, int>()
                .GetByIdAsync(postId);

            if (post == null)
                throw new Exception("Post not found.");

            if (post.AuthorId != userId)
                throw new Exception("You are not authorized to delete this post.");

            await _unitOfWork.Repository<Post, int>()
                .DeleteByIdAsync(postId);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            var posts = await _unitOfWork.Repository<Post, int>()
                .FindAsync(p => p.Status == PostStatus.Approved);

            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public async Task<PostDetailsDto?> GetPostByIdAsync(int postId)
        {
            var post = await _unitOfWork.Repository<Post, int>()
                .GetByIdAsync(postId);

            if (post == null)
                throw new Exception("Post not found.");

            return _mapper.Map<PostDetailsDto>(post);
        }

        public async Task<IEnumerable<PostDto>> GetPostsByAuthorIdAsync(int AuthorId)
        {
            var posts = await _unitOfWork.Repository<Post, int>().FindAsync(p => p.AuthorId == AuthorId);

            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public async Task<IEnumerable<PostDto>> GetPostsByCategoryIdAsync(int CategoryId)
        {
            var posts = await _unitOfWork.Repository<Post, int>().FindAsync(p => p.CategoryId == CategoryId);

            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }
        public async Task<PostDto?> SubmitPostAsync(int postId, int userId)
        {
            var post = await _unitOfWork.Repository<Post, int>()
                .FirstOrDefaultAsync(p => p.Id == postId && p.AuthorId == userId);

            if (post == null)
                throw new Exception("Post not found.");

            if (post.Status != PostStatus.Draft)
                throw new Exception("Only draft posts can be submitted.");

            post.Status = PostStatus.PendingApproval;
            post.SubmittedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Post, int>().Update(post);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto?> ApprovePostAsync(int PostId, int UserId)
        {
            var post = await _unitOfWork.Repository<Post, int>()
                .FirstOrDefaultAsync(p => p.Id == PostId && p.Status == PostStatus.PendingApproval);
            if (post == null)
                throw new Exception("Post not found or not pending approval.");

            post.Status = PostStatus.Approved;
            _unitOfWork.Repository<Post, int>().Update(post);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PostDto>(post);
        }
        public Task<IEnumerable<PostDto>> GetPostsByTagsAsync(int TagId)
        {
            throw new NotImplementedException();
        }

        public async Task<PostDto?> RejectPostAsync(int PostId, int UserId)
        {
            var post = await _unitOfWork.Repository<Post, int>()
                .FirstOrDefaultAsync(p => p.Id == PostId && p.Status == PostStatus.PendingApproval);
            if (post == null)
                throw new Exception("Post not found or not pending approval.");

            post.Status = PostStatus.Approved;
            _unitOfWork.Repository<Post, int>().Update(post);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<PostDto>(post);
        }
    }
}
