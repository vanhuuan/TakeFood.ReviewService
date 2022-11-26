namespace TakeFood.ReviewsService.ViewModel.Dtos.Review
{
    public class ReviewPagingResponse
    {
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<ManageReviewDto> ManageReviews { get; set; }
    }
}
