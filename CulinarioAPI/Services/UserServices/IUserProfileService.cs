using CulinarioAPI.Dtos.UserDtos;

namespace CulinarioAPI.Services.UserServices
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(string username);
        Task<UserDetailsDto> GetUserDetailsAsync(string username);
        Task<bool> UpdateUserProfileAsync(string username, UserProfileUpdateDto profileDto);
    }
}
