using CulinarioAPI.Models.UserModels;

namespace CulinarioAPI.Repositories.UserRepositories
{
    public interface IUserProfileRepository
    {
        Task AddUserProfileAsync(UserProfile userProfile);
        Task<UserProfile> GetUserProfileByUsernameAsync(string username);
        Task UpdateUserProfileAsync(UserProfile profile);
        Task<bool> UserProfileExistsAsync(string username);
    }
}
