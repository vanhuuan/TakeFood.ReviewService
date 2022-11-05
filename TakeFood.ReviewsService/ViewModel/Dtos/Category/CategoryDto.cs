using System.Text.Json.Serialization;

namespace TakeFood.ReviewsService.ViewModel.Dtos.Category
{
    public class CategoryDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
