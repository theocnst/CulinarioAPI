using CulinarioAPI.Dtos;

namespace CulinarioAPI.Services
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(int id);
        Task<bool> UpdateUserProfileAsync(int id, UserProfileUpdateDto profileDto);
    }
}
