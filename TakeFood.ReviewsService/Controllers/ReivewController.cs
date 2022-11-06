using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TakeFood.ReviewsService.Middleware;
using TakeFood.ReviewsService.Service;
using TakeFood.ReviewsService.ViewModel.Dtos.Review;

namespace TakeFood.ReviewsService.Controllers;

public class ReviewController : BaseController
{
    public IReviewService ReviewService { get; set; }
    public IJwtService JwtService { get; set; }
    public ReviewController(IReviewService reviewService, IJwtService jwtService)
    {
        this.ReviewService = reviewService;
        JwtService = jwtService;
    }

    [HttpPost]
    [Authorize]
    [Route("CreateReview")]
    public async Task<IActionResult> AddReviewAsync([FromBody] CreateReviewDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.ErrorCount);
            }
            await ReviewService.CreateReview(dto, GetId());
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Authorize]
    [Route("GetReviews")]
    public async Task<IActionResult> GetReviewAsync([Required] int index, [Required] string storeId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rs = await ReviewService.GetListReview(index, storeId);
            return Ok(rs);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    public string GetId()
    {
        String token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
        return JwtService.GetId(token);
    }
    public string GetId(string token)
    {
        return JwtService.GetId(token);
    }

}
