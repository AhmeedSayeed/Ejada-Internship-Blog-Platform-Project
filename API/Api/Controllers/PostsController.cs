using API.Application.Services;
using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace API.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IPostImgService _postImgService;
        public PostsController(IPostService postService, IPostImgService postImgService)
        {
            _postService = postService;
            _postImgService = postImgService;
        }
        [HttpPost]
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<PostDto>> CreatePost(CreatePostDto postDto)
        {

            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );
            if (userId == 0)
            { return Unauthorized(); }
            var post = await _postService.CreatePostAsync(postDto, userId);
            return Ok(post);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetAllPosts()
        {
            var post = await _postService.GetAllPostsAsync();
            return Ok(post);
        }
        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PostDto>> GetPostById(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            return Ok(post);
        }
        [HttpPut]
        [Route("{id}/Submit")]
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<PostDto>> SubmitPostById(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (userId == 0)
            { return Unauthorized(); }
            var post = await _postService.SubmitPostAsync(id, userId);
            return Ok(post);
        }
        [HttpPut]
        [Route("{id}/Approve")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PostDto>> ApprovePostById(int id)
        {

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            var post = await _postService.ApprovePostAsync(id, userId);
            return Ok(post);
        }
        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<UpdatePostDto>> UpdatePost(UpdatePostDto postDto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            var post = await _postService.UpdatePostAsync(postDto, userId);
            return Ok(post);
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Author")]
        public async Task<ActionResult> DeletePost(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            var post = await _postService.DeletePostAsync(id, userId);
            return Ok(post);

        }
        [HttpPut]
        [Route("{id}/Reject")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PostDto>> RejectPostById(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            await _postService.RejectPostAsync(id, userId);
            return Ok(post);
        }
        [HttpPost("images")]
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> UploadImage([FromForm] PostImageDto postimg)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var imageUrl = await _postImgService.UploadPostImageAsync(userId, postimg);

            return Ok(new
            {
                ImageUrl = imageUrl
            });
        }

        [HttpGet("{PostId}/images")]
        public async Task<IActionResult> GetImages(int PostId)
        {
            var images = await _postImgService.GetPostImagesAsync(PostId);

            return Ok(images);
        }
        [HttpDelete("images/{imageId}")]
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _postImgService.DeletePostImageAsync(imageId, userId);

            return NoContent();
        }
    }
}
