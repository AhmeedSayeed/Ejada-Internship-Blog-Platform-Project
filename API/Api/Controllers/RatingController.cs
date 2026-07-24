using Blog_Project.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;


        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        [Route("{postId}/Rating")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<RatingDetailsDto>> CreateRating(int postId, CreateRatingDto ratingDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var createdRating = await _ratingService.CreateRatingAsync(postId, ratingDto, userId);
            return Ok(createdRating);
        }
        [HttpPut]
        [Route("{postId}/Rating")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<RatingDetailsDto>> UpdateRating(int postId, UpdateRatingDto ratingDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var Rating = await _ratingService.UpdateRatingAsync(postId, ratingDto, userId);
            return Ok(Rating);
        }
    }
    }
