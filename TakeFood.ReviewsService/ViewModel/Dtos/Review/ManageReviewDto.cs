namespace TakeFood.ReviewsService.ViewModel.Dtos.Review
{
    public class ManageReviewDto
    {
        public string Code { get; set; }
        public int Star { get; set; }
        public string Description { get; set; }
        public string OrderID { get; set; }
        public List<String> Imgs { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
