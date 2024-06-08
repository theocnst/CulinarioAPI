using CulinarioAPI.Dtos.UserDtos;

namespace CulinarioAPI.Services.UserServices
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(int id);
        Task<bool> UpdateUserProfileAsync(int id, UserProfileUpdateDto profileDto);
    }
}
