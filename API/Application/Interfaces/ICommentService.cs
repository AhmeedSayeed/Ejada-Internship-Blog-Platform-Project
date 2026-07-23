namespace Blog_Project.Application.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDetailsDto> CreateCommentAsync(CreateCommentDto commentDto, int userId);
        Task<CommentDetailsDto?> UpdateCommentAsync(UpdateCommentDto commentDto, int userId);

        Task<CommentDetailsDto> GetCommentsAsync(int postId);

        Task<UpdateCommentStatusDto?> ApproveCommentAsync(int CommentId);
        Task<UpdateCommentStatusDto?> RejectCommentAsync(int CommentId);
        Task DeleteCommentAsync(int CommentId, int userId);

    }
}
