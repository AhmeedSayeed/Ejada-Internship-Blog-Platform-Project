using API.Application.Services;
using Blog_Project.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {

            _commentService = commentService;
        }

        [HttpGet]
        [Route("{postId}/comments")]
        public async Task<ActionResult>GetCommentsAsync(int postId) {

            var comments = await _commentService.GetCommentsAsync(postId);
            return Ok(comments);
        }
        [HttpPost]
        [Route("{postId}/comments")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<CommentDetailsDto>>CreateComment(int postId,CreateCommentDto commentDetailsDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var createdComment = await _commentService.CreateCommentAsync(postId,commentDetailsDto,userId);
            return Ok(createdComment);
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Reader,Admin")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            var comment = await _commentService.DeleteCommentAsync(id, userId);
            return Ok(comment);

        }
        [HttpPut]
        [HttpPut]
        [Route("{id}/Reject")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CommentDetailsDto>> RejectCommentById(int id)
        {
          
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            var comment=await _commentService.RejectCommentAsync(id);
            return Ok(comment);
        }
        [HttpPut]
        [HttpPut]
        [Route("{id}/Approve")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CommentDetailsDto>> ApproveCommentById(int id)
        {

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            var comment =await _commentService.ApproveCommentAsync(id);
            return Ok(comment);
        }
        [HttpPut]
        [HttpPut]
        [Route("{id}/Flag")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<CommentDetailsDto>> FlagCommentById(int id)
        {

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            var comment = await _commentService.FlagCommentAsync(id);
            return Ok(comment);
        }

    }
}
