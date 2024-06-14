using CulinarioAPI.Dtos.UserDtos;
using CulinarioAPI.Models.UserModels;

namespace CulinarioAPI.Repositories.UserRepositories
{
    public interface IUserProfileRepository
    {
        Task AddUserProfileAsync(UserProfile userProfile);
        Task<UserDetailsDto> GetUserDetailsByUsernameAsync(string username);
        Task<UserProfile> GetUserProfileByUsernameAsync(string username);
        Task UpdateUserProfileAsync(UserProfile profile);
        Task<bool> UserProfileExistsAsync(string username);
        Task<bool> AddFriendAsync(FriendshipDto friendshipDto);
        Task<bool> RemoveFriendAsync(FriendshipDto friendshipDto);

    }
}
