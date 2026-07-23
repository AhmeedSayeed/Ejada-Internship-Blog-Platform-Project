namespace Blog_Project.Application.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDetailsDto> CreateCommentAsync(int postId,CreateCommentDto commentDto, int userId);
        Task<CommentDetailsDto?> UpdateCommentAsync(UpdateCommentDto commentDto, int userId);

        Task<List<CommentDetailsDto>> GetCommentsAsync(int postId);

        Task<CommentDetailsDto?> ApproveCommentAsync(int CommentId);
        Task<CommentDetailsDto?> RejectCommentAsync(int CommentId);
    
        Task<CommentDetailsDto?> FlagCommentAsync(int CommentId);
        Task<bool> DeleteCommentAsync(int CommentId, int userId);

    }
}
