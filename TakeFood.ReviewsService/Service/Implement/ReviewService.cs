using MongoDB.Driver;
using TakeFood.ReviewsService.Model.Entities.Order;
using TakeFood.ReviewsService.Model.Entities.Review;
using TakeFood.ReviewsService.Model.Entities.Store;
using TakeFood.ReviewsService.Model.Entities.User;
using TakeFood.ReviewsService.Model.Repository;
using TakeFood.ReviewsService.ViewModel.Dtos.Review;

namespace TakeFood.ReviewsService.Service.Implement;

public class ReviewService : IReviewService
{
    private IMongoRepository<Review> reviewRepository { get; set; }
    private IMongoRepository<Order> orderRepository { get; set; }
    private IMongoRepository<Store> storeRepository { get; set; }
    private IMongoRepository<User> userRepository { get; set; }
    public ReviewService(IMongoRepository<Review> reviewRepository, IMongoRepository<Order> orderRepository, IMongoRepository<Store> storeRepository,
        IMongoRepository<User> userRepository)
    {
        this.reviewRepository = reviewRepository;
        this.orderRepository = orderRepository;
        this.storeRepository = storeRepository;
        this.userRepository = userRepository;
    }
    public async Task CreateReview(CreateReviewDto dto, string uid)
    {
        var order = await orderRepository.FindOneAsync(x => x.Id == dto.OrderId);
        if (order == null || order.UserId != uid)
        {
            throw new Exception("Order's note exist");
        }
        var store = await storeRepository.FindOneAsync(x => x.Id == order.StoreId);
        store.NumReiview++;
        store.SumStar += dto.Star;
        var count = await reviewRepository.CountAsync(x => x.OrderId == dto.OrderId);
        if (count > 0)
        {
            throw new Exception("You reviewed");
        }
        var review = new Review()
        {
            Star = dto.Star,
            OrderId = dto.OrderId,
            Imgs = dto.Images
        };
        await storeRepository.UpdateAsync(store);
        await reviewRepository.InsertAsync(review);
    }

    public async Task<List<ReviewDetailDto>> GetListReview(int index, string orderId)
    {
        var list = new List<ReviewDetailDto>();
        var reviews = await reviewRepository.GetPagingAsync(Builders<Review>.Filter.Eq(x => x.OrderId, orderId), index, 10);
        foreach (var review in reviews.Data)
        {
            var order = await orderRepository.FindByIdAsync(review.OrderId);
            if (order == null) continue;
            var user = await userRepository.FindByIdAsync(order.UserId);
            var detail = new ReviewDetailDto()
            {
                Description = review.Description,
                Images = review.Imgs,
                Star = review.Star,
            };

            if (user != null)
            {
                detail.UserName = user.Name;
            }
            else
            {
                detail.UserName = "Unknow";
            }
            list.Add(detail);
        }
        return list;
    }
}
