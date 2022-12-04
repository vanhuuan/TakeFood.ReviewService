using TakeFood.ReviewsService.ViewModel.Dtos.Review;

namespace TakeFood.ReviewsService.Service;

public interface IReviewService
{
    Task CreateReview(CreateReviewDto dto, string uid);
    Task<List<ReviewDetailDto>> GetListReview(int index, string storeId);
    Task<ReviewDetailDto> GetUserReview(string storeId, string uid);
    Task<ReviewPagingResponse> GetManageReview(GetPagingReviewDto dto, string storeID);
    Task<List<ManageReviewDto>> GetAllReviews(string storeId);
}
