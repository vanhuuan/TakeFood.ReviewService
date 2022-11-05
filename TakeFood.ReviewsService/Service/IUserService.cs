using TakeFood.ReviewsService.ViewModel.Dtos;
using TakeFood.ReviewsService.ViewModel.Dtos.User;

namespace TakeFood.ReviewsService.Service;

/// <summary>
/// IUserService
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Get user by user id
    /// </summary>
    /// <returns></returns>
    Task<UserViewDto> GetUserByIdAsync(string id);

    /// <summary>
    /// Create new user
    /// </summary>
    /// <returns></returns>
    Task<UserViewDto> CreateUserAsync(CreateUserDto createUserDto);

    /// <summary>
    /// Sign in by phone number or email
    /// </summary>
    /// <returns></returns>
    Task<UserViewDto> SignIn(LoginDto loginDto);

    /// <summary>
    /// Active user via active link
    /// </summary>
    /// <returns></returns>
    Task Active(string token);
    Task UpdateRole(string userID);
    Task<Boolean> HasStore(string userID);
}
