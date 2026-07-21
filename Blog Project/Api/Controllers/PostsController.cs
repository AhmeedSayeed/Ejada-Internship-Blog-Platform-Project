using Blog_Project.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Blog_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpPost]
        [Authorize("Author")]
        public async Task<ActionResult<PostDto>> CreatePost(CreatePostDto postDto)
        {
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"{claim.Type} = {claim.Value}");
            }
       

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
            var post= await _postService.GetAllPostsAsync();
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
        [Route("Submet/{id}")]
        [Authorize("Author")]
        public async Task<ActionResult<PostDto>> SubmitPostById(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (userId == 0)
            { return Unauthorized(); }
            await _postService.SubmitPostAsync(id, userId);
            return Ok(post);
        }
        [HttpPut]
        [Route("Approve/{id}")]
        [Authorize("Admin")]
        public async Task<ActionResult<PostDto>> ApprovePostById(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            await _postService.ApprovePostAsync(id, userId);
            return Ok(post);
        }
        [HttpPut]
        [Route("Update")]
        [Authorize("Author")]
        public async Task<ActionResult<UpdatePostDto>> UpdatePost(UpdatePostDto postDto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            var post = await _postService.UpdatePostAsync(postDto,userId);
            return Ok(post);
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize("Author")]
        public async Task<ActionResult> DeletePost(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            var post = await _postService.DeletePostAsync(id, userId);
            return Ok(post);

        }
        [HttpPut]
        [Route("Reject/{id}")]
        [Authorize("Admin")]
        public async Task<ActionResult<PostDto>> RejectPostById(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (userId == 0)
            { return Unauthorized(); }
            await _postService.RejectPostAsync(id, userId);
            return Ok(post);
        }

    }
}
