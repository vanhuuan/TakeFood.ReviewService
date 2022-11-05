using TakeFood.ReviewsService.ViewModel.Dtos.Category;

namespace TakeFood.ReviewsService.Service
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(String id);
        Task CreateCategory(CategoryDto category);
        Task UpdateCategory(string id, CategoryDto category);
        Task DeleteCategory(String id);
    }
}
