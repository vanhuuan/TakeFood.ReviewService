namespace TakeFood.ReviewsService.ViewModel.Dtos.Review;

public class ReviewDetailDto
{
    public string Description { get; set; }
    public string UserName { get; set; }
    public int Star { get; set; }
    public DateTime Created { get; set; }
    public List<String> Images { get; set; }
}
