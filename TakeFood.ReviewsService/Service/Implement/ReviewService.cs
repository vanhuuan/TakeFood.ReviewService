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

    public async Task<List<ReviewDetailDto>> GetListReview(int index, string storeId)
    {
        var list = new List<ReviewDetailDto>();
        var orderIds = orderRepository.FindAsync(x => x.StoreId == storeId).Result.Select(x => x.Id);
        var reviews = reviewRepository.FindAsync(x => orderIds.Contains(x.OrderId)).Result.Take(index * 10).TakeLast(10);
        foreach (var review in reviews)
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

    public async Task<ReviewPagingResponse> GetManageReview(GetPagingReviewDto dto, string storeID)
    {
        var filter = CreateFilter(dto.StartDate, dto.EndDate, storeID);
        if (dto.PageNumber <= 0 || dto.PageSize <= 0)
        {
            throw new Exception("Pagenumber or pagesize can not be  zero or negative");
        }
        var rs = await reviewRepository.GetPagingAsync(filter, dto.PageNumber - 1, dto.PageSize);

        var list = new List<ManageReviewDto>();
        foreach (var review in rs.Data)
        {
            list.Add(new ManageReviewDto()
            {
                Code = review.Id[19..],
                Star = review.Star,
                Description = review.Description,
                OrderID = review.OrderId,
                CreatedDate = review.CreatedDate
            });
        }

        list = list.OrderBy(x => x.CreatedDate).ToList();
        list.Reverse();

        var info = new ReviewPagingResponse()
        {
            Total = rs.Count,
            PageIndex = dto.PageNumber,
            PageSize = dto.PageSize,
            ManageReviews = list
        };

        return info;
    }

    private FilterDefinition<Review> CreateFilter(DateTime? startDate, DateTime? endDate, string storeId)
    {
        var orders = orderRepository.Find(x => x.StoreId == storeId);
        List<string> listOrderID = new List<string>();
        if (orders != null)
        {
            foreach (var order in orders)
            {
                listOrderID.Add(order.Id);
            }
        }
        if (listOrderID.Count > 0)
        {
            FilterDefinition<Review> filter = Builders<Review>.Filter.Eq(x => x.OrderId, listOrderID[0]);
            for (int i = 1; i < listOrderID.Count; i++)
            {
                filter |= Builders<Review>.Filter.Eq(x => x.OrderId, listOrderID[i]);
            }
            if (startDate != null && endDate != null)
            {
                filter &= Builders<Review>.Filter.Gte(x => x.CreatedDate, startDate);
                filter &= Builders<Review>.Filter.Lte(x => x.CreatedDate, endDate);
            }
            return filter;
        }

        return null;
    }

    public async Task<List<ManageReviewDto>> GetAllReviews(string storeId)
    {
        List<Order> orders = (List<Order>)await orderRepository.FindAsync(x => x.StoreId == storeId);
        List<ManageReviewDto> list = new List<ManageReviewDto>();

        foreach (var order in orders)
        {
            Review review = await reviewRepository.FindOneAsync(x => x.OrderId == order.Id);
            if (review != null) list.Add(new ManageReviewDto()
            {
                Code = review.Id.Substring(19),
                Star = review.Star,
                Description = review.Description,
                OrderID = review.OrderId,
                CreatedDate = review.CreatedDate
            });
        }

        list = list.OrderBy(x => x.CreatedDate).ToList();
        list.Reverse();

        return list;
    }
}
