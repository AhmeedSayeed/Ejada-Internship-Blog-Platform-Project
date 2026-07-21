

namespace Blog_Project.Application.Interfaces
{
    public interface IPostService
    {
        Task<PostDto> CreatePostAsync(CreatePostDto PostDto, int UserId);
        Task<PostDto?> UpdatePostAsync(UpdatePostDto PostDto, int UserId);
        Task<bool> DeletePostAsync(int PostId, int UserId);
        Task<PostDetailsDto?> GetPostByIdAsync(int PostId);

        Task<PostDto?> SubmitPostAsync(int PostId, int UserId);
        Task<PostDto?> ApprovePostAsync(int PostId, int UserId);

        Task<IEnumerable<PostDto>> GetAllPostsAsync();

        Task<IEnumerable<PostDto>> GetPostsByAuthorIdAsync(int AuthorId);

        Task<IEnumerable<PostDto>> GetPostsByCategoryIdAsync(int CategoryId);
        Task<IEnumerable<PostDto>> GetPostsByTagsAsync(int TagId);
    }
}
