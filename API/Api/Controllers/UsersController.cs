using API.Application.DTOs;
using API.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Api.Controllers
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
                return NotFound(ApiErrors.From(result.Error));

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
                return result.Error.Code switch
                {
                    "User.NotFound" => NotFound(ApiErrors.From(result.Error)),
                    "Password.ChangeFailed" => BadRequest(ApiErrors.From(result.Error)),
                    _ => BadRequest(ApiErrors.From(result.Error))
                };
            }

            return NoContent();
        }

        // POST api/<UsersController>/me/profile-image
        [HttpPost("me/profile-image")]
        [Authorize]
        public async Task<IActionResult> UpdateProfileImage(IFormFile file)
        {
            var id = GetUserId();

            var result = await userProfileService.UpdateProfileImageAsync(id, file);
            if (!result.IsSuccess)
            {
                return result.Error.Code switch
                {
                    "User.NotFound" => NotFound(ApiErrors.From(result.Error)),
                    "File.Invalid" => BadRequest(ApiErrors.From(result.Error)),
                    _ => BadRequest(ApiErrors.From(result.Error))
                };
            }

            return Ok(result.Value);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await userProfileService.GetPublicProfileAsync(id);
            if (!result.IsSuccess)
                return NotFound(ApiErrors.From(result.Error));

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
                return result.Error.Code switch
                {
                    "Follower.NotFound" => NotFound(ApiErrors.From(result.Error)),
                    "TargetUser.NotFound" => NotFound(ApiErrors.From(result.Error)),
                    "Follow.Invalid" => BadRequest(ApiErrors.From(result.Error)),
                    "Follow.AlreadyExists" => BadRequest(ApiErrors.From(result.Error)),
                    _ => BadRequest(ApiErrors.From(result.Error))
                };
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
                return result.Error.Code switch
                {
                    "Follower.NotFound" => NotFound(ApiErrors.From(result.Error)),
                    "TargetUser.NotFound" => NotFound(ApiErrors.From(result.Error)),
                    "Follow.NotFound" => NotFound(ApiErrors.From(result.Error)), // was missing — this was the live bug
                    _ => BadRequest(ApiErrors.From(result.Error))
                };
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
                return NotFound(ApiErrors.From(result.Error));

            return Ok(result.Value);
        }

        // GET api/<UsersController>/5/followers
        [HttpGet("{id}/followers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFollowers([FromRoute] int id)
        {
            var result = await userProfileService.GetFollowersAsync(id);

            if (!result.IsSuccess)
                return NotFound(ApiErrors.From(result.Error));

            return Ok(result.Value);
        }
    }
}
