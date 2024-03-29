﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

    [HttpGet]
    [Route("GetReview")]
    public async Task<IActionResult> GetUserReviewAsync([Required] string orderId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rs = await ReviewService.GetUserReview(orderId, GetId());
            return Ok(rs);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    [Route("GetPaging")]
    public async Task<JsonResult> GetPagingReview(GetPagingReviewDto dto, string storeID)
    {
        try
        {
            var rs = await ReviewService.GetManageReview(dto, storeID);
            return new JsonResult(rs);
        }
        catch (Exception e)
        {
            return new JsonResult(e.Message);
        }
    }

    [HttpGet]
    [Route("GetAllReview")]
    public async Task<JsonResult> GetAllReviews(string storeID)
    {
        try
        {
            var rs = await ReviewService.GetAllReviews(storeID);
            return new JsonResult(rs);
        }
        catch (Exception e)
        {
            return new JsonResult(e);
        }
    }

    public string GetId()
    {
        string id = HttpContext.Items["Id"]!.ToString()!;
        return id;
    }

}
