using Blog_Project.Application.DTOs;
using Blog_Project.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserProfileService userProfileService) : ControllerBase
    {
        private int GetUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Int32.Parse(id!);
        }

        // GET: api/<UsersController>/me
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            var id = GetUserId();

            var result = await userProfileService.GetMyProfileAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        // PUT api/<UsersController>/me
        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto dto)
        {
            var id = GetUserId();

            var result = await userProfileService.UpdateProfileAsync(id, dto);
            if (!result.IsSuccess)
            {
                if (result.Error.Code == "User.NotFound")
                    return NotFound(result.Error);
                if (result.Error.Code == "Password.ChangeFailed")
                    return BadRequest(result.Error);
            }

            return NoContent();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await userProfileService.GetPublicProfileAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        // POST api/<UsersController>/5/follow
        [HttpPost("{id}/follow")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> PostFollow([FromRoute] int id)
        {
            var followerId = GetUserId();

            var result = await userProfileService.FollowAsync(followerId, id);
            if (!result.IsSuccess)
            {
                if (result.Error.Code == "Follower.NotFound")
                    return NotFound(result.Error);

                if (result.Error.Code == "TargetUser.NotFound")
                    return NotFound(result.Error);

                if (result.Error.Code == "Follow.Invalid")
                    return BadRequest(result.Error);

                if (result.Error.Code == "Follow.AlreadyExists")
                    return BadRequest(result.Error);
            }

            return Ok();
        }

        // DELETE api/<UsersController>/5/follow
        [HttpDelete("{id}/follow")]
        [Authorize]
        public async Task<IActionResult> DeleteFollow([FromRoute] int id)
        {
            var followerId = GetUserId();

            var result = await userProfileService.UnfollowAsync(followerId, id);
            if (!result.IsSuccess)
            {
                if (result.Error.Code == "Follower.NotFound")
                    return NotFound(result.Error);

                if (result.Error.Code == "TargetUser.NotFound")
                    return NotFound(result.Error);

                if (result.Error.Code == "Follow.Invalid")
                    return BadRequest(result.Error);
            }

            return Ok();
        }

        // GET api/<UsersController>/me/following
        [HttpGet("me/following")]
        [Authorize]
        public async Task<IActionResult> GetFollowing()
        {
            var userId = GetUserId();

            var result = await userProfileService.GetFollowingAsync(userId);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        // GET api/<UsersController>/5/followers
        [HttpGet("{id}/followers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFollowers([FromRoute] int id)
        {
            var result = await userProfileService.GetFollowersAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }
    }
}
