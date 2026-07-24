using Blog_Project.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost]
        [Route("{postId}/Like")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<CommentDetailsDto>> CreateLike(int postId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var createdLike = await _likeService.CreateLikeAsync(postId, userId);
            return Ok(createdLike);
        }
        [HttpDelete]
        [Route("{postId}/Like")]
        [Authorize(Roles = "Reader,Admin")]
        public async Task<ActionResult> DeleteLike(int postId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            var like = await _likeService.DeleteLikeAsync(postId, userId);
            return Ok(like);

        }
        [HttpGet]
        [Route("{postId}/Likes")]
        public async Task<ActionResult<int>> GetLikes(int postId)
        {
            var likesCount = await _likeService.GetLikesAsync(postId);
            return Ok(likesCount);
        }
    }
}
